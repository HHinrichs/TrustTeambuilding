* Encoding: UTF-8.
SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE SEPARATE BY WiesahenihreMitspieleraus.

*Spearman-Korrelation.
NONPAR CORR
  /VARIABLES=N_CogTrust N_TeamEffectiveness
  /PRINT=SPEARMAN TWOTAIL NOSIG
  /MISSING=PAIRWISE.

*Spearman-Korrelation.
NONPAR CORR
  /VARIABLES=N_CogTrust N_TeamCommunication
  /PRINT=SPEARMAN TWOTAIL NOSIG
  /MISSING=PAIRWISE.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=N_TeamEffectiveness N_CogTrust MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE
  /FITLINE TOTAL=NO.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: N_TeamEffectiveness=col(source(s), name("N_TeamEffectiveness"))
  DATA: N_CogTrust=col(source(s), name("N_CogTrust"))
  GUIDE: axis(dim(1), label("N_TeamEffectiveness"))
  GUIDE: axis(dim(2), label("Kognitives Vertrauen"))
  GUIDE: text.title(label("Einfaches Streudiagramm  von Kognitives Vertrauen Schritt: ",
    "N_TeamEffectiveness"))
  ELEMENT: point(position(N_TeamEffectiveness*N_CogTrust))
END GPL.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=N_TeamCommunication N_CogTrust MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE
  /FITLINE TOTAL=NO.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: N_TeamCommunication=col(source(s), name("N_TeamCommunication"))
  DATA: N_CogTrust=col(source(s), name("N_CogTrust"))
  GUIDE: axis(dim(1), label("N_TeamCommunication"))
  GUIDE: axis(dim(2), label("Kognitives Vertrauen"))
  GUIDE: text.title(label("Einfaches Streudiagramm  von Kognitives Vertrauen Schritt: ",
    "N_TeamCommunication"))
  ELEMENT: point(position(N_TeamCommunication*N_CogTrust))
END GPL.
