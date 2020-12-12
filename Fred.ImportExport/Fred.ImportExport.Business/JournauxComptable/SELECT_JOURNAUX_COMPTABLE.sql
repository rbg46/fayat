SELECT  TRIM(RSTE) As CodeSociete, 
        TRIM(RJAL) As CodeJournal, 
        TRIM(JNOM) As NomJournal, 
        TRIM(JTYPE) As TypeJournal
FROM axfile.fan060p1
WHERE RSTE = '{0}' AND CAST((JFERA || '-' || JFERM || '-' || JFERJ) As Date) > Cast('{1}' AS DATE)