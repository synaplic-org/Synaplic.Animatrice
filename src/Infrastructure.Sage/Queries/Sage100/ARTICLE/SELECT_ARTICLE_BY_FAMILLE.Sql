select 
AR_Ref CodeArticle,
AR_CodeBarre CodeBarre,
AR_Design Designation,
FA_CodeFamille CodeFamille,
AR_SuiviStock SuiviStock,
AR_Sommeil  EnSommiel 
from F_ARTICLE 
where FA_CodeFamille in @CodeFamilles