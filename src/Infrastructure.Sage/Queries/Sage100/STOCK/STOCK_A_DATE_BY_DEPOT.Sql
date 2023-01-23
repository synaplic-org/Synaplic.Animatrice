SELECT A.STD_ARREF        Reference,
       A.STD_ARDESING       Designation,
       A.STD_LSNOSERIE      Lot,
       ART.FA_CodeFamille CodeFamille,
       SUM(CASE
               WHEN A.STD_DLDATEBL <= @DateInventaire THEN
                   A.STD_QTE
               ELSE
                   0
           END
           )              QteStockDate,
       SUM(CASE
               WHEN A.STD_DLDATEBL > @DateInventaire THEN
                   A.STD_QTE
               ELSE
                   0
           END
           )              QteMouvement
FROM (
         SELECT ARTICLE.DE_No                                AS STD_DENO,
                FDEP.DE_Intitule                             AS STD_DEINTITULE,
                FART.AR_Ref                                  AS STD_ARREF,
                FART.AR_Design                               AS STD_ARDESING,
                ARTICLE.LS_NOSERIE                           AS STD_LSNOSERIE,
                NULLIF(ARTICLE.LS_PEREMPTION, '01/01/1900')  AS STD_PEREMPTION,
                NULLIF(ARTICLE.LS_FABRICATION, '01/01/1900') AS STD_FABRICATION,
                ARTICLE.DO_Piece                             AS STD_DOPIECE,
                ARTICLE.DO_Date                              AS STD_DODATE,
                ISNULL(ARTICLE.DL_DateBL, ARTICLE.DO_Date)   AS STD_DLDATEBL,
                SUM(ARTICLE.QTE)                             AS STD_QTE,
                ARTICLE.DO_Type,
                ARTICLE.DL_MvtStock,
                FART.AR_SuiviStock
         FROM (
                  SELECT SOUSREQSELLIGNE.DE_No,
                         SOUSREQSELLIGNE.cbAR_Ref,
                         SOUSREQSELLIGNE.AG_No1,
                         SOUSREQSELLIGNE.AG_No2,
                         LS_NOSERIE,
                         LS_PEREMPTION,
                         LS_FABRICATION,
                         QTE QTE,
                         SOUSREQSELLIGNE.DO_Piece,
                         SOUSREQSELLIGNE.DO_Type,
                         SOUSREQSELLIGNE.DL_MvtStock,
                         SOUSREQSELLIGNE.DL_TypePL,
                         SOUSREQSELLIGNE.DO_Date,
                         SOUSREQSELLIGNE.DL_DateBL
                  FROM (
                           SELECT FDOC.DE_No,
                                  FDOC.cbAR_Ref,
                                  FDOC.AG_No1,
                                  FDOC.AG_No2,
                                  NULL       LS_NOSERIE,
                                  NULL       LS_PEREMPTION,
                                  NULL       LS_FABRICATION,
                                  CASE DL_MvtStock
                                      WHEN 1 THEN
                                          CASE
                                              WHEN DO_Type = 27
                                                  OR DO_Type = 4 THEN
                                                  DL_Qte
                                              ELSE
                                                  CASE
                                                      WHEN DL_TypePL > 0
                                                          OR
                                                           (
                                                                       DL_Qte < 0
                                                                   AND DO_Domaine <> 2
                                                               ) THEN
                                                          -DL_Qte
                                                      ELSE
                                                          DL_Qte
                                                      END
                                              END
                                      WHEN 3 THEN
                                          CASE
                                              WHEN DO_Type = 14 THEN
                                                  -DL_Qte
                                              ELSE
                                                  CASE
                                                      WHEN DL_TypePL > 0
                                                          OR DL_Qte < 0 THEN
                                                          DL_Qte
                                                      ELSE
                                                          -DL_Qte
                                                      END
                                              END
                                      ELSE
                                          0
                                      END AS QTE,
                                  CASE
                                      WHEN DO_Type = 20 THEN
                                          DL_MontantHT
                                      WHEN DL_MvtStock = 1 THEN
                                          ROUND(DL_CMUP * ABS(DL_Qte), 2)
                                      WHEN DL_MvtStock = 3 THEN
                                          ROUND(DL_CMUP * ABS(DL_Qte), 2)
                                      ELSE
                                          DL_CMUP
                                      END AS CMUP,
                                  CASE
                                      WHEN (
                                                       DL_MvtStock = 4
                                                   OR DL_MvtStock = 2
                                               )
                                          AND AR_SuiviStock <> 2 THEN
                                          0
                                      ELSE
                                          CASE
                                              WHEN DO_Type = 20 THEN
                                                  DL_MontantHT
                                              WHEN DL_MvtStock = 3
                                                  OR DL_MvtStock = 1 THEN
                                                  ROUND(DL_PrixRU * ABS(DL_Qte), 2)
                                              ELSE
                                                  DL_PrixRU
                                              END
                                      END AS PR,
                                  CASE DL_MvtStock
                                      WHEN 1 THEN
                                          CASE
                                              WHEN DO_Type = 27
                                                  AND DL_Qte < 0 THEN
                                                  -1
                                              ELSE
                                                  1
                                              END
                                      WHEN 3 THEN
                                          -1
                                      WHEN 4 THEN
                                          CASE
                                              WHEN DO_Domaine = 1
                                                  AND
                                                   (
                                                               DL_TypePL = 2
                                                           OR DL_TypePL = 3
                                                       ) THEN
                                                  1
                                              ELSE
                                                  -1
                                              END
                                      ELSE
                                          1
                                      END AS SENS,
                                  FDOC.DO_Piece,
                                  FDOC.DO_Type,
                                  FDOC.DL_MvtStock,
                                  FDOC.DL_TypePL,
                                  FDOC.DO_Date,
                                  FDOC.DL_DateBL
                           FROM F_DOCLIGNE FDOC
                                    INNER JOIN F_ARTICLE FART
                                               ON (FDOC.cbAR_Ref = FART.cbAR_Ref)
                           WHERE (FDOC.DL_MvtStock <> 0)
                             AND (
                                       FART.AR_SuiviStock >= 2
                                   AND FART.AR_SuiviStock <= 4
                               )
                             AND (
                                       FDOC.DO_Type < 5
                                   OR FDOC.DO_Type > 5
                               )
                           UNION ALL
                           SELECT FDOC.DE_No,
                                  FDOC.cbAR_Ref,
                                  FDOC.AG_No1,
                                  FDOC.AG_No2,
                                  FLOT.LS_NoSerie,
                                  FLOT.LS_Peremption,
                                  FLOT.LS_Fabrication,
                                  CASE DL_MvtStock
                                      WHEN 1 THEN
                                          CASE
                                              WHEN DO_Type = 27
                                                  OR DO_Type = 4 THEN
                                                  DL_Qte
                                              ELSE
                                                  CASE
                                                      WHEN DL_TypePL > 0
                                                          OR
                                                           (
                                                                       DL_Qte < 0
                                                                   AND DO_Domaine <> 2
                                                               ) THEN
                                                          -DL_Qte
                                                      ELSE
                                                          DL_Qte
                                                      END
                                              END
                                      WHEN 3 THEN
                                          CASE
                                              WHEN DO_Type = 14 THEN
                                                  -DL_Qte
                                              ELSE
                                                  CASE
                                                      WHEN DL_TypePL > 0
                                                          OR DL_Qte < 0 THEN
                                                          DL_Qte
                                                      ELSE
                                                          -DL_Qte
                                                      END
                                              END
                                      ELSE
                                          0
                                      END AS QTE,
                                  CASE
                                      WHEN DO_Type = 20 THEN
                                          DL_MontantHT
                                      WHEN DL_MvtStock = 3
                                          OR DL_MvtStock = 1 THEN
                                          ROUND(DL_CMUP * ABS(DL_Qte), 2)
                                      ELSE
                                          DL_CMUP
                                      END AS CMUP,
                                  CASE
                                      WHEN DL_MvtStock = 4
                                          OR DL_MvtStock = 2
                                               AND AR_SuiviStock <> 2 THEN
                                          0
                                      ELSE
                                          CASE
                                              WHEN DO_Type = 20 THEN
                                                  DL_MontantHT
                                              WHEN DL_MvtStock = 3
                                                  OR DL_MvtStock = 1 THEN
                                                  ROUND(DL_PrixRU * ABS(DL_Qte), 2)
                                              ELSE
                                                  DL_PrixRU
                                              END
                                      END AS PR,
                                  CASE DL_MvtStock
                                      WHEN 1 THEN
                                          CASE
                                              WHEN DO_Type = 27
                                                  AND DL_Qte < 0 THEN
                                                  -1
                                              ELSE
                                                  1
                                              END
                                      WHEN 3 THEN
                                          -1
                                      WHEN 4 THEN
                                          CASE
                                              WHEN DO_Domaine = 1
                                                  AND
                                                   (
                                                               DL_TypePL = 2
                                                           OR DL_TypePL = 3
                                                       ) THEN
                                                  1
                                              ELSE
                                                  -1
                                              END
                                      ELSE
                                          1
                                      END AS SENS,
                                  FDOC.DO_Piece,
                                  FDOC.DO_Type,
                                  FDOC.DL_MvtStock,
                                  FDOC.DL_TypePL,
                                  FDOC.DO_Date,
                                  FDOC.DL_DateBL
                           FROM F_DOCLIGNE FDOC
                                    INNER JOIN F_ARTICLE FART
                                               ON (FDOC.cbAR_Ref = FART.cbAR_Ref)
                                    INNER JOIN
                                (
                                    SELECT DL_NoIn DL_NO,
                                           LS_Peremption,
                                           LS_Fabrication,
                                           LS_NoSerie
                                    FROM F_LOTSERIE
                                    WHERE DL_NoOut = 0
                                    UNION
                                    SELECT DL_NoOut DL_NO,
                                           LS_Peremption,
                                           LS_Fabrication,
                                           LS_NoSerie
                                    FROM F_LOTSERIE
                                    WHERE DL_NoOut > 0
                                ) FLOT
                                ON (FDOC.DL_No = FLOT.DL_NO)
                           WHERE (
                                   (
                                           (
                                                       FART.AR_SuiviStock = 1
                                                   OR FART.AR_SuiviStock = 5
                                               )
                                           AND FLOT.LS_NoSerie IS NOT NULL
                                       )
                                   OR
                                   (
                                               FART.AR_SuiviStock = 5
                                           AND FLOT.LS_NoSerie IS NOT NULL
                                       )
                               )
                             AND FDOC.DL_MvtStock <> 0
                             AND (
                                       FDOC.DO_Type < 5
                                   OR FDOC.DO_Type > 5
                               )
                       ) SOUSREQSELLIGNE
              ) ARTICLE
                  INNER JOIN F_DEPOT FDEP
                             ON (ARTICLE.DE_No = FDEP.DE_No)
                  INNER JOIN F_ARTICLE FART
                             ON (ARTICLE.cbAR_Ref = FART.cbAR_Ref)
         GROUP BY NULLIF(ARTICLE.LS_PEREMPTION, '01/01/1900'),
                  NULLIF(ARTICLE.LS_FABRICATION, '01/01/1900'),
                  ISNULL(ARTICLE.DL_DateBL, ARTICLE.DO_Date),
                  ARTICLE.DE_No,
                  FDEP.DE_Intitule,
                  FART.AR_Ref,
                  FART.AR_Design,
                  ARTICLE.LS_NOSERIE,
                  ARTICLE.DO_Piece,
                  ARTICLE.DO_Date,
                  ARTICLE.DO_Type,
                  ARTICLE.DL_MvtStock,
                  FART.AR_SuiviStock
     ) A inner  join F_ARTICLE ART on A.STD_ARREF = ART.AR_Ref
         

WHERE A.STD_DENO = @DepotNo
GROUP BY A.STD_ARREF, A.STD_ARDESING,
         A.STD_LSNOSERIE,ART.FA_CodeFamille
HAVING SUM(CASE
               WHEN A.STD_DLDATEBL <= @DateInventaire THEN
                   A.STD_QTE
               ELSE
                   0
    END
           ) <> 0
    OR SUM(CASE
               WHEN A.STD_DLDATEBL > @DateInventaire THEN
                   A.STD_QTE
               ELSE
                   0
    END
           ) <> 0




 

