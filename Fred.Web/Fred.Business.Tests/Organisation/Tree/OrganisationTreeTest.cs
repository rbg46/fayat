using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Fred.Common.Tests.Data.Organisation.Mock;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Common.Tests.Data.Role.Mock;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.Entities.Organisation.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Organisation.Tree
{
    [TestClass]
    public class OrganisationTreeTest
    {
        private OrganisationBase holding;
        private OrganisationBase groupeRzb;
        private OrganisationBase groupeFes;
        private OrganisationBase societeRzb;
        private OrganisationBase societeBianco;
        private OrganisationBase societeSatelec;
        private OrganisationBase etablissementBiancoRzb;
        private OrganisationBase ci411100SocieteRzb;
        private OrganisationBase ci411100SocieteBianco;
        private OrganisationBase ci411100SocieteSatelec;
        private OrganisationTree organisationTree;

        [TestInitialize]
        public void TestInitialize()
        {
            holding = OrganisationTreeMocks.GetHoldingFayat();
            groupeRzb = OrganisationTreeMocks.GetGroupeRzb();
            groupeFes = OrganisationTreeMocks.GetGroupeFES();
            societeRzb = OrganisationTreeMocks.GetSocieteRzb();
            societeBianco = OrganisationTreeMocks.GetSocieteBianco();
            societeSatelec = OrganisationTreeMocks.GetSocieteSatelec();
            etablissementBiancoRzb = OrganisationTreeMocks.GetEtablissementBiancoRzb();
            ci411100SocieteRzb = OrganisationTreeMocks.GetCi_411100_SocieteRzb();
            ci411100SocieteBianco = OrganisationTreeMocks.GetCi_411100_SocieteBianco();
            ci411100SocieteSatelec = OrganisationTreeMocks.GetCi_411100_SocieteSatelec();

            var oranisations = new List<OrganisationBase>
            {
                holding,
                groupeRzb,
                groupeFes,
                societeRzb,
                societeBianco,
                societeSatelec,
                etablissementBiancoRzb,
                ci411100SocieteRzb,
                ci411100SocieteBianco,
                ci411100SocieteSatelec
            };

            organisationTree = new OrganisationTree(oranisations);

        }

        [TestMethod]
        public void OrganisationTree_GetParents_Test()
        {
            var parents = organisationTree.GetParents(ci411100SocieteRzb.OrganisationId);


            Assert.IsTrue(parents.Count == 3, "Il devrait y avoir 3 parents et l'element courant ne doit pas etre inclus.");
            Assert.IsTrue(parents.ElementAt(0).OrganisationId == societeRzb.OrganisationId, "Le premier parent devrait etre la societe Rzb.");
            Assert.IsTrue(parents.ElementAt(1).OrganisationId == groupeRzb.OrganisationId, "Le deuxieme parent devrait etre le groupe Rzb.");
            Assert.IsTrue(parents.ElementAt(2).OrganisationId == holding.OrganisationId, "Le 3ieme parent devrait etre la holding.");
        }

        [TestMethod]
        public void OrganisationTree_GetParentsWithCurrent_Test()
        {
            var parents = organisationTree.GetParentsWithCurrent(ci411100SocieteRzb.OrganisationId);

            Assert.IsTrue(parents.Count == 4, "Il devrait y avoir 3 parents et l'element courant doit etre inclus.");
            Assert.IsTrue(parents.ElementAt(0).OrganisationId == ci411100SocieteRzb.OrganisationId, "Le premier parent devrait etre le ci Rzb.");
            Assert.IsTrue(parents.ElementAt(1).OrganisationId == societeRzb.OrganisationId, "Le deuxieme parent devrait etre la societe Rzb.");
            Assert.IsTrue(parents.ElementAt(2).OrganisationId == groupeRzb.OrganisationId, "Le 3ieme parent devrait etre le groupe Rzb.");
            Assert.IsTrue(parents.ElementAt(3).OrganisationId == holding.OrganisationId, "Le 4ieme parent devrait etre la holding.");
        }

        [TestMethod]
        public void OrganisationTree_GetParentsWithCurrentUntilGroupe_Test()
        {
            var parents = organisationTree.GetParentsWithCurrentUntilGroupe(ci411100SocieteRzb.OrganisationId);

            Assert.IsTrue(parents.Count == 3, "Il devrait y avoir 3 parents et l'element courant doit etre inclus.");
            Assert.IsTrue(parents.ElementAt(0).OrganisationId == ci411100SocieteRzb.OrganisationId, "Le premier parent devrait etre le ci Rzb.");
            Assert.IsTrue(parents.ElementAt(1).OrganisationId == societeRzb.OrganisationId, "Le deuxieme parent devrait etre la societe Rzb.");
            Assert.IsTrue(parents.ElementAt(2).OrganisationId == groupeRzb.OrganisationId, "Le 3ieme parent devrait etre le groupe Rzb.");
            Assert.IsTrue(parents.Any(x => x.IsHolding()) == false, "Il ne devrait pas y avoir de holding.");
        }

        [TestMethod]
        public void OrganisationTree_GetParentsWithCurrentUntilSociete_Test()
        {
            var parents = organisationTree.GetParentsWithCurrentUntilSociete(ci411100SocieteRzb.OrganisationId);

            Assert.IsTrue(parents.Count == 2, "Il devrait y avoir 1 parent et l'element courant doit etre inclus.");
            Assert.IsTrue(parents.ElementAt(0).OrganisationId == ci411100SocieteRzb.OrganisationId, "Le premier parent devrait etre le ci Rzb.");
            Assert.IsTrue(parents.ElementAt(1).OrganisationId == societeRzb.OrganisationId, "Le deuxieme parent devrait etre la societe Rzb.");

            Assert.IsTrue(parents.Any(x => x.IsHolding()) == false, "Il ne devrait pas y avoir de holding.");
            Assert.IsTrue(parents.Any(x => x.IsHolding()) == false, "Il ne devrait pas y avoir de groupe.");
        }

        [TestMethod]
        public void OrganisationTree_GetFirstParent_Test()
        {
            var parent = organisationTree.GetFirstParent(ci411100SocieteRzb.OrganisationId);

            Assert.IsTrue(parent != null, "Le parent du ci est la societe RZB.");
            Assert.IsTrue(parent.IsSociete(), "Le parent du ci est la societe RZB.");
            Assert.IsTrue(parent.OrganisationId == societeRzb.OrganisationId, "Le parent du ci est la societe RZB.");
            Assert.IsTrue(parent.Id == societeRzb.Id, "Le parent du ci est la societe RZB.");
        }

        [TestMethod]
        public void OrganisationTree_GetFirstParent_Test2()
        {
            var parent = organisationTree.GetFirstParent(societeBianco.OrganisationId);

            Assert.IsTrue(parent != null, "Le parent du ci est la holding.");
            Assert.IsTrue(parent.IsGroupe(), "Le parent de la societe BIANCO est la groupe RZB.");
            Assert.IsTrue(parent.OrganisationId == groupeRzb.OrganisationId, "Le parent du ci est la groupe RZB.");
            Assert.IsTrue(parent.Id == groupeRzb.Id, "Le parent du ci est la groupe RZB.");
        }

        [TestMethod]
        public void OrganisationTree_GetAllParentsOfCi_Test()
        {
            var parents = organisationTree.GetAllParentsOfCi(ci411100SocieteRzb.Id);

            Assert.AreEqual(4, parents.Count, "Il devrait y avoir 3 parents et l'element courant doit etre inclus.");
            Assert.AreEqual(ci411100SocieteRzb.OrganisationId, parents.ElementAt(0).OrganisationId, "Le premier parent devrait etre le ci Rzb.");
            Assert.AreEqual(societeRzb.OrganisationId, parents.ElementAt(1).OrganisationId, "Le deuxieme parent devrait etre la societe Rzb.");
        }

        [TestMethod]
        public void OrganisationTree_GetAllParentsOfCi_Test2()
        {
            // le ci avec le ciId = 0 n'existe pas
            var parents = organisationTree.GetAllParentsOfCi(0);

            parents.Count.Should().Be(0);

        }

        [TestMethod]
        public void OrganisationTree_GetAllCisOfSociete_Test()
        {
            var cisRzb = organisationTree.GetAllCisOfSociete(societeRzb.Id);

            Assert.AreEqual(1, cisRzb.Count, "Il devrait y avoir un ci pour la societe RZB.");


            var cisBianco = organisationTree.GetAllCisOfSociete(societeBianco.Id);

            Assert.AreEqual(1, cisBianco.Count, "Il devrait y avoir 1 ci pour la societe Bianco.");


            var cisSatelec = organisationTree.GetAllCisOfSociete(societeSatelec.Id);

            Assert.AreEqual(1, cisSatelec.Count, "Il devrait y avoir 1 ci pour la societe Satelec.");
        }

        [TestMethod]
        public void OrganisationTree_GetCI_Test()
        {
            var retrievedCi = organisationTree.GetCi(groupeRzb.Id, societeBianco.Code, ci411100SocieteBianco.Code);

            Assert.AreEqual(retrievedCi, ci411100SocieteBianco, "Le ci devrait etre le ci de Bianco.");
            Assert.AreEqual(retrievedCi.Id, ci411100SocieteBianco.Id, "Le ci devrait etre le ci de Bianco.");
            Assert.AreEqual(retrievedCi.OrganisationId, ci411100SocieteBianco.OrganisationId, "Le ci devrait etre le ci de Bianco.");

        }

        [TestMethod]
        public void OrganisationTree_GetCI_Error_Test()
        {
            organisationTree.PrintDebug(holding);

            var retrievedCi = organisationTree.GetCi(groupeRzb.Id, string.Empty, ci411100SocieteBianco.Code);

            Assert.AreEqual(null, retrievedCi, "Aucun ci ne dvrait etre trouvé car il manque le code de la societe .");


            retrievedCi = organisationTree.GetCi(groupeFes.Id, societeBianco.Code, ci411100SocieteBianco.Code);

            Assert.AreEqual(null, retrievedCi, "Aucun ci ne dvrait etre trouvé car il manque le groupeID n'est pas celui de la societe Bianco(rzb).");


            retrievedCi = organisationTree.GetCi(groupeRzb.Id, societeBianco.Code, string.Empty);

            Assert.AreEqual(null, retrievedCi, "Aucun ci ne dvrait etre trouvé car il manque le code du ci");
        }

        [TestMethod]
        public void OrganisationTree_GetCI_ByID_Test()
        {
            organisationTree.PrintDebug(holding);

            var retrievedCi = organisationTree.GetCi(0);

            Assert.AreEqual(null, retrievedCi, "Aucun ci ne dvrait etre trouvé car il n'y pas de ci avec le ciid = 0.");


            retrievedCi = organisationTree.GetCi(ci411100SocieteRzb.Id);

            Assert.AreEqual(ci411100SocieteRzb.Id, retrievedCi.Id, "Le ci de Rzb devrait remonter.");
            Assert.AreEqual(ci411100SocieteRzb.OrganisationId, retrievedCi.OrganisationId, "Le ci de Rzb devrait remonter.");
        }

        [TestMethod]
        public void OrganisationTree_GetEtablissementComptable_Test()
        {
            organisationTree.PrintDebug(holding);

            var result = organisationTree.GetEtablissementComptable(etablissementBiancoRzb.Id);

            Assert.AreEqual(etablissementBiancoRzb.Id, result.Id, "L'etablissement de la societe bianco de Rzb devrait remonter.");
            Assert.AreEqual(etablissementBiancoRzb.OrganisationId, etablissementBiancoRzb.OrganisationId, "L'etablissement de la societe bianco de Rzb devrait remonter.");
        }

        [TestMethod]
        public void OrganisationTree_GetAllEtablissementComptableForUser_Test()
        {

            var affectationRzb = new AffectationBase()
            {
                AffectationId = 0,
                OrganisationId = societeRzb.OrganisationId,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS
            };
            // creation d'une affectation sur la societe rzb
            societeRzb.Affectations.Add(affectationRzb);

            organisationTree.PrintDebug(holding);

            var result = organisationTree.GetAllEtablissementComptableForUser(affectationRzb.UtilisateurId);

            Assert.AreEqual(0, result.Count, "L'etablissement est sur la societe bianco alors que l'affectation est sur la societe rzb, donc aucun etablissement ne remonte.");

            var affectationBianco = new AffectationBase()
            {
                AffectationId = 1,
                OrganisationId = societeBianco.OrganisationId,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_BIANCO_REPONSABLE_DE_PAIE,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS
            };
            // creation d'une affectation sur la societe bianco
            societeBianco.Affectations.Add(affectationBianco);


            result = organisationTree.GetAllEtablissementComptableForUser(affectationRzb.UtilisateurId);


            Assert.AreEqual(1, result.Count, "L'etablissement est sur la societe bianco de Rzb  donc un etablissement remonte.");
            Assert.AreEqual(etablissementBiancoRzb.OrganisationId, result.First(), "L'etablissement est sur la societe bianco de Rzb donc un etablissement remonte.");
        }


        [TestMethod]
        public void OrganisationTree_GetAllCiOrganisationBaseForUser_Test()
        {

            var affectationRzb = new AffectationBase()
            {
                AffectationId = 0,
                OrganisationId = societeRzb.OrganisationId,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS
            };

            // creation d'une affectation sur la societe rzb
            societeRzb.Affectations.Add(affectationRzb);

            var affectationBianco = new AffectationBase()
            {
                AffectationId = 1,
                OrganisationId = societeBianco.OrganisationId,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_BIANCO_REPONSABLE_DE_PAIE,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS
            };
            // creation d'une affectation sur la societe bianco
            societeBianco.Affectations.Add(affectationBianco);


            var result = organisationTree.GetAllCiOrganisationBaseForUser(affectationRzb.UtilisateurId);


            Assert.AreEqual(2, result.Count, "Il y a un ci sur bianco et un ci sur rzb");

        }

        [TestMethod]
        public void OrganisationTree_GetAllSocietes_Test()
        {
            var result = organisationTree.GetAllSocietes();

            Assert.AreEqual(3, result.Count, "Il y a un 3 societes");
        }


        [TestMethod]
        public void OrganisationTree_GetAllSocietesForGroupe_Test()
        {
            var result = organisationTree.GetAllSocietesForGroupe(this.groupeRzb.Id);

            Assert.AreEqual(2, result.Count, "Il y a un 2 societes");
        }

        [TestMethod]
        public void OrganisationTree_GetSocieteParent_Test()
        {
            var result = organisationTree.GetSocieteParent(this.ci411100SocieteBianco.OrganisationId);

            Assert.AreEqual(this.societeBianco.OrganisationId, result.OrganisationId, "La societe parent du ci de bianco est la societe bianco.");
        }

        [TestMethod]
        public void OrganisationTree_GetGroupeParentOfCi_Test()
        {
            var result = organisationTree.GetGroupeParentOfCi(this.ci411100SocieteBianco.Id);

            Assert.AreEqual(this.groupeRzb.OrganisationId, result.OrganisationId, "Le groupe parent du ci de bianco est le groupe Rzb.");
        }

        [TestMethod]
        public void OrganisationTree_GetSocieteParentOfCi_Test()
        {
            var result = organisationTree.GetSocieteParentOfCi(this.ci411100SocieteBianco.Id);

            Assert.AreEqual(this.societeBianco.OrganisationId, result.OrganisationId, "La societe parent du ci de bianco est la societe bianco.");
        }

        [TestMethod]
        public void OrganisationTree_GetGroupeParentOfSociete_Test()
        {
            var result = organisationTree.GetGroupeParentOfSociete(this.societeRzb.Id);

            Assert.AreEqual(this.groupeRzb.OrganisationId, result.OrganisationId, "La groupe parent de la societe rzb est le groupe rzb.");
        }




        [TestMethod]
        public void Verifie_Que_il_y_a_autant_d_organisation_que_de_nodes()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            Assert.IsTrue(organisationTree.Nodes.Count == resultRepo.Count, "Il faut autant de nodes que d'organisations.");

        }

        [TestMethod]
        public void Verifie_Que_la_holding_contient_tous_les_autres_organisations()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var holdingNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.IsHolding());

            var childrenNode = holdingNode.PostOrder();

            Assert.IsTrue(childrenNode.Count == resultRepo.Count, "PostOrder donne tous les enfants + lui meme");

        }


        [TestMethod]
        public void Verifie_Que_la_methode_levelorder_trie_dans_l_ordre()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var holdingNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.IsHolding());

            var childrenNode = holdingNode.LevelOrder();

            Assert.IsTrue(childrenNode.Count == resultRepo.Count, "LevelOrder donne tous les enfants + lui meme");
            Assert.IsTrue(childrenNode.FirstOrDefault().IsHolding(), "Le premier est la holding");
            Assert.IsTrue(childrenNode.ElementAt(1).IsPole(), "Le second est le pole");
            Assert.IsTrue(childrenNode.ElementAt(2).IsGroupe(), "Le troisieme est le groupe");
        }

        [TestMethod]
        public void Verifie_Que_la_methode_PreOrder_trie_dans_l_ordre()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var holdingNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.IsHolding());

            var childrenNode = holdingNode.PreOrder();

            Assert.IsTrue(childrenNode.Count == resultRepo.Count, "LevelOrder donne tous les enfants + lui meme");
            Assert.IsTrue(childrenNode.FirstOrDefault().IsHolding(), "Le premier est la holding");
            Assert.IsTrue(childrenNode.ElementAt(1).IsPole(), "Le second est le pole");
            Assert.IsTrue(childrenNode.ElementAt(2).IsGroupe(), "Le troisieme est le groupe");
        }


        [TestMethod]
        public void Verifie_Que_la_societe_a_trois_enfants()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var societeNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.IsSociete());

            var childrenNode = societeNode.LevelOrder();

            Assert.IsTrue(childrenNode.Count == 3, "LevelOrder donne tous les enfants + lui meme");
            Assert.IsTrue(childrenNode.FirstOrDefault().IsSociete(), "Le premier est la societe");
            Assert.IsTrue(childrenNode.Where(c => c.IsCi()).Count() == 2, "Il devrait y avoir 2 ci");
        }

        [TestMethod]
        public void Verifie_Que_la_societe_a_trois_enfants2()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var lastCiNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.OrganisationId == 7);

            var childrenNode = lastCiNode.LevelOrder();

            Assert.IsTrue(childrenNode.Count == 1, "LevelOrder donne tous les enfants + lui meme");
            Assert.IsTrue(childrenNode.FirstOrDefault().IsCi(), "C'est le seul de type ci");
        }

        [TestMethod]
        public void Verifie_Que_la_methode_parent_retourne_les_parents()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var lastCiNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.OrganisationId == 7);

            var parentsNode = lastCiNode.Parents();

            Assert.IsTrue(parentsNode.Count == 3, "Parents donne tous les parents dans l'ordre trouveé");
            Assert.IsTrue(parentsNode.FirstOrDefault().IsGroupe(), "Lpremierparent est le groupe");
        }


        [TestMethod]
        public void Verifie_Que_la_l_utilisateur_1_a_tous_les_nodes_de_la_societeetdunautreci()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var userOrganisation = organisationTree.GetOrganisationTreeForUser(1);

            var lastCiNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.OrganisationId == 7);

            var ciWithoutExplicitAffectationForUser = organisationTree.Nodes.FirstOrDefault(n => n.Data.OrganisationId == 5);

            Assert.IsTrue(userOrganisation.Nodes.Count == 4, "Pour l'utilisateur 1, il y a 4 nodes car il est sur le societe et un ci");

            Assert.IsTrue(lastCiNode != null, "Le ci associé au groupe devrait apparaitre");

            Assert.IsTrue(ciWithoutExplicitAffectationForUser != null, "Le ci sans affectation explicit devrait apparaitre.");
        }


        [TestMethod]
        public void Verifie_Que_Level_Order_retourne_tous_les_cis()
        {
            var resultRepo = OrganisationBaseMocks.GetOrganisationBases();

            var organisationTree = new OrganisationTree(resultRepo);

            var holdingNode = organisationTree.Nodes.FirstOrDefault(n => n.Data.IsHolding());

            var ciChildren = holdingNode.LevelOrder(o => o.IsCi()).ToList();

            Assert.IsTrue(ciChildren.Count == 3, "LevelOrder donne tous les ci enfants");

        }

    }
}
