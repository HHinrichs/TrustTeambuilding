* Encoding: UTF-8.

*Diskreptive Statistik.
DATASET ACTIVATE DataSet2.
EXAMINE VARIABLES=TeamRoundsDone N_TeamGenTrustScore BY TeamID
  /PLOT BOXPLOT NPPLOT
  /COMPARE GROUPS
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

SORT CASES  BY TeamID.
SPLIT FILE LAYERED BY TeamID.

*Spearman-Korrelation berechnen.
NONPAR CORR
  /VARIABLES=TeamRoundsDone N_TeamGenTrustScore
  /PRINT=SPEARMAN TWOTAIL NOSIG
  /MISSING=PAIRWISE.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=TeamRoundsDone N_TeamGenTrustScore MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE
  /FITLINE TOTAL=NO.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: TeamRoundsDone=col(source(s), name("TeamRoundsDone"))
  DATA: N_TeamGenTrustScore=col(source(s), name("N_TeamGenTrustScore"))
  GUIDE: axis(dim(1), label("TeamRoundsDone"))
  GUIDE: axis(dim(2), label("N_TeamGenTrustScore"))
  GUIDE: text.title(label("Einfaches Streudiagramm  von N_TeamGenTrustScore Schritt: ",
    "TeamRoundsDone"))
  ELEMENT: point(position(TeamRoundsDone*N_TeamGenTrustScore))
END GPL.

SPLIT FILE OFF.

