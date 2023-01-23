select CO_No CodeCollaborateur,CO_Nom Nom, CO_Prenom Prenom,
CO_Telephone Telephone,CO_TelPortable TelPortable,CO_EMail Email,CO_Vendeur IsVendeur,
CO_Caissier IsCaissier,CO_Acheteur isAcheteur,CO_ChargeRecouvr IsChargeRecover,CO_Financier IsFinancier
,CO_Transmission EnTransmission  from F_COLLABORATEUR

where CO_NO = @Representant