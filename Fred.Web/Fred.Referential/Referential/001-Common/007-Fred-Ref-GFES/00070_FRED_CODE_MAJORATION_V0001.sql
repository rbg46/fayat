 if exists (select CodeMajorationId from FRED_CODE_MAJORATION where Code = 'TNH1')
 BEGIN
		 update FRED_CODE_MAJORATION SET IsHeureNuit = 1  where  Code = 'TNH1';
 END

 if exists (select CodeMajorationId from FRED_CODE_MAJORATION where Code = 'TNH2')
 BEGIN
		 update FRED_CODE_MAJORATION SET IsHeureNuit = 1  where  Code = 'TNH2';
 END