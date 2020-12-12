-- La permission contextuelle est passée a 1 sans raison ... 

UPDATE F SET PERMISSIONCONTEXTUELLE = 0
FROM FRED_PERMISSION F
WHERE 1=1
AND PERMISSIONKEY ='button.enabled.close.period.index'