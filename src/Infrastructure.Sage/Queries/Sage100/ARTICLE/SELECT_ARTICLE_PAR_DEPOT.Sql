select F_ARTICLE.AR_Ref Reference,F_ARTICLE.AR_Design Designation,AS_QteSto QuantiteReel,AS_MontSto Montant,arts.DE_NO CodeDepot,dpt.DE_Intitule Depot  from F_ARTICLE 
inner  join F_ARTSTOCK arts on arts.AR_Ref = F_ARTICLE.ar_ref
inner join  F_DEPOT dpt on dpt.de_no = arts.DE_No