select FA_CodeFamille CodeFamille, FA_Intitule Intitule,FA_Central Central,unt.U_Intitule Unite,
isnull(cbCreation,'')DateCreation,
cbModification DateModification from F_FAMILLE
inner join P_UNITE unt on unt.cbIndice =  F_FAMILLE.FA_UniteVen