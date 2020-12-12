using System;
using System.Collections.Generic;
using System.IO;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Framework.Exceptions;
using Fred.Framework.Tool;

namespace Fred.Business.PieceJointe.Services
{
    public class PieceJointeService : IPieceJointeService
    {
        private readonly IPieceJointeStockagePhysiqueRepository pieceJointeStockagePhysiqueRepository;
        private readonly IPieceJointeRepository pieceJointeRepository;
        private readonly IPieceJointeCommandeRepository pieceJointeCommandeRepository;
        private readonly IPieceJointeReceptionRepository pieceJointeReceptionRepository;
        private readonly IUnitOfWork uow;

        public PieceJointeService(
            IPieceJointeStockagePhysiqueRepository pieceJointeStockagePhysiqueRepository,
            IPieceJointeRepository pieceJointeRepository,
            IPieceJointeCommandeRepository pieceJointeCommandeRepository,
            IPieceJointeReceptionRepository pieceJointeReceptionRepository,
            IUnitOfWork uow)
        {
            this.pieceJointeStockagePhysiqueRepository = pieceJointeStockagePhysiqueRepository;
            this.pieceJointeRepository = pieceJointeRepository;
            this.pieceJointeCommandeRepository = pieceJointeCommandeRepository;
            this.pieceJointeReceptionRepository = pieceJointeReceptionRepository;
            this.uow = uow;
        }

        /// <summary>
        /// Récupérer toutes les pièces jointes selon le type de l'entité et l'identifiant
        /// </summary>
        /// <param name="typeEntite">Type de l'entité (commande / réception / ...)</param>
        /// <param name="entiteId">Identifiant de l'entité</param>
        /// <returns>Liste des pièces jointes trouvées</returns>
        public List<PieceJointeEnt> GetPiecesJointes(PieceJointeTypeEntite typeEntite, int entiteId)
        {
            List<PieceJointeEnt> result = null;
            switch (typeEntite)
            {
                case PieceJointeTypeEntite.Commande:
                    result = pieceJointeCommandeRepository.GetPiecesJointes(entiteId);
                    break;
                case PieceJointeTypeEntite.Reception:
                    result = pieceJointeReceptionRepository.GetPiecesJointes(entiteId);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Récupérer une pièce jointe par l'ID
        /// </summary>
        /// <param name="commonFolder">Chemin commun de stockage</param>
        /// <param name="pieceJointeId">Identifiant de la pièce jointe</param>
        /// <returns>Pièce jointe trouvée</returns>
        public PieceJointeEnt GetPieceJointeWithFile(string commonFolder, int pieceJointeId)
        {
            PieceJointeEnt pj = pieceJointeRepository.FindById(pieceJointeId);
            if (pj == null)
            {
                throw new FredBusinessNotFoundException("Pièce jointe non trouvée.");
            }

            // Combine path
            string fullPath;
            try
            {
                fullPath = Path.Combine(commonFolder, pj.RelativePath);
            }
            catch (Exception ex)
            {
                throw new FredBusinessNotFoundException("Le fichier correspondant à la pièce jointe est introuvable. CommonFolder(" + commonFolder ?? "NULL" + ") RelativePath(" + pj.RelativePath ?? "NULL" + ")", ex);
            }

            // Vérifier existence physique
            if (!File.Exists(fullPath))
            {
                throw new FredBusinessNotFoundException("Le fichier correspondant à la pièce jointe est introuvable. FullPath(" + fullPath + ")");
            }

            // Récupérer le fichier physique
            pj.PieceJointeArray = pieceJointeStockagePhysiqueRepository.GetFile(fullPath);

            return pj;
        }

        /// <summary>
        /// Ajouter une pièce jointe et l'attacher à une entité (Commande / Depense / ...)
        /// </summary>
        /// <param name="userId">L'id de l'utilisateur</param>
        /// <param name="typeEntite">Type de l'entité à lquelle sera attachée la pièce jointe</param>
        /// <param name="entiteId">L'Id de l'entité à lquelle sera attachée la pièce jointe</param>
        /// <param name="commonFolder">Répertoire commun de sauvegarde</param>
        /// <param name="specificPath">Répertoire spécifique de sauvegarde</param>
        /// <param name="fileName">Nom du fichier</param>
        /// <param name="localPath">Stockage temporaire</param>
        /// <returns>Pièce jointe ajouté</returns>
        public PieceJointeEnt Add(int userId, PieceJointeTypeEntite typeEntite, int entiteId, string commonFolder, string specificPath, string fileName, string localPath)
        {
            // Génération du nom de sauvegarde
            string guid = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(fileName);

            // Calculer taille
            int sizeInKo = FileTool.GetFileSizeInKo(localPath);

            // Combine path
            string targeRelativetPath = Path.Combine(specificPath, DateTime.UtcNow.ToString("yyyyMM"), guid + extension);

            // Sauvegarde physique du fichier
            pieceJointeStockagePhysiqueRepository.MoveFile(localPath, Path.Combine(commonFolder, targeRelativetPath));

            // Insérer une pièce jointe
            PieceJointeEnt pieceJointe = pieceJointeRepository.Insert(new PieceJointeEnt()
            {
                Libelle = fileName,
                RelativePath = targeRelativetPath,
                SizeInKo = sizeInKo,
                AuteurCreationId = userId,
                DateCreation = DateTime.UtcNow
            });

            // Attacher pièce jointe à une entité
            switch (typeEntite)
            {
                case PieceJointeTypeEntite.Commande:
                    AttachPieceJointeToCommande(entiteId, pieceJointe.PieceJointeId);
                    break;
                case PieceJointeTypeEntite.Reception:
                    AttachPieceJointeToReception(entiteId, pieceJointe.PieceJointeId);
                    break;
            }

            // Sauvegarder les modifications
            uow.Save();

            return pieceJointe;
        }

        /// <summary>
        /// Effacer pièce jointe et l'attachement en cascade à une entité
        /// </summary>
        /// <param name="commonFolder">Chemin commun de stockage</param>
        /// <param name="pieceJointeId">Identifiant de la pièce jointe</param>
        public void Delete(string commonFolder, int pieceJointeId)
        {
            // Récupérer infos pièce jointe
            PieceJointeEnt pj = pieceJointeRepository.FindById(pieceJointeId);
            if (pj == null)
            {
                throw new FredBusinessNotFoundException("La pièce jointe est introuvable.");
            }

            // Combine path
            string fullPath = Path.Combine(commonFolder, pj.RelativePath);

            // Effacer physiquement le fichier
            try
            {
                pieceJointeStockagePhysiqueRepository.DeleteFile(fullPath);
            }
            catch (IOException ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"{System.Reflection.MethodBase.GetCurrentMethod()} : La suppression du fichier {pj.RelativePath} a échoué.", ex);

                throw new FredBusinessNotFoundException("La pièce jointe est introuvable.");
            }

            // Effacer la pièce jointe & la relation en cascade
            pieceJointeRepository.DeleteById(pieceJointeId);

            // Sauvegarder les modifications
            uow.Save();
        }


        /// <summary>
        /// Attacher une pièce jointe à une commande
        /// </summary>
        /// <param name="entiteId">L'identifiant de la commande</param>
        /// <param name="pieceJointeId">L'identifiant de la pièce jointe</param>
        private void AttachPieceJointeToCommande(int entiteId, int pieceJointeId)
        {
            pieceJointeCommandeRepository.Insert(new PieceJointeCommandeEnt()
            {
                CommandeId = entiteId,
                PieceJointeId = pieceJointeId
            });

            // Sauvegarder les modifications
            uow.Save();
        }

        /// <summary>
        /// Attacher une pièce jointe à une réception
        /// </summary>
        /// <param name="entiteId">L'identifiant de la réception</param>
        /// <param name="pieceJointeId">L'identifiant de la pièce jointe</param>
        private void AttachPieceJointeToReception(int entiteId, int pieceJointeId)
        {
            pieceJointeReceptionRepository.Insert(new PieceJointeReceptionEnt()
            {
                ReceptionId = entiteId,
                PieceJointeId = pieceJointeId
            });

            // Sauvegarder les modifications
            uow.Save();
        }
    }
}
