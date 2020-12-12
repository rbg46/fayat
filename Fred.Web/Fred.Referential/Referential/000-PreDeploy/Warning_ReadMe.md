# !! ATTENTION !!

Les scripts dans ce dossier sont joués avant la mise à jour (ou le déploiement) de Fred.
Si vous mettez un script ici, vous devez prendre en compte que : 
* Les migrations EF seront joués APRES votre script et non avant. Si le script s'appuie donc sur le nouveau schéma, il plantera.
* Comme les scripts dans ce dossier sont joués AVANT le déploiement, si le déploiement plante les scripts auront quand même été joués.
* Le reste des scripts dans les autres dossiers parents genre Common/Release/Prod sont joués APRES la mise à jour de l'application.

La séquence est la suivante : 

1. Exécution des scripts dans le dossier PreDeploy
1. Mise à jour des binaires de l'application
1. Exécution des migrations EF
1. Exécution des scripts dans les dossiers Common/Release/Prod etc...

