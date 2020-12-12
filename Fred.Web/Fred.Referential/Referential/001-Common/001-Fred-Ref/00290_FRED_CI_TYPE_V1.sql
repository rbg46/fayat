SET IDENTITY_INSERT  FRED_CI_TYPE ON;

INSERT INTO  FRED_CI_TYPE (CITypeId, Designation, RessourceKey, Code) VALUES  (1, 'Affaire', 'CI_Search_CIType_Affaire', 'A')
INSERT INTO  FRED_CI_TYPE (CITypeId, Designation, RessourceKey, Code) VALUES  (2, 'Etude', 'CI_Search_CIType_Etude', 'E')
INSERT INTO  FRED_CI_TYPE (CITypeId, Designation, RessourceKey, Code) VALUES  (3, 'Section', 'CI_Search_CIType_Section', 'S')

SET IDENTITY_INSERT FRED_CI_TYPE  OFF;