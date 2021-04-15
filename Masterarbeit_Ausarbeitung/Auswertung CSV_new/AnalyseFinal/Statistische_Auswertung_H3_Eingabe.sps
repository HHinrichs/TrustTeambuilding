* Encoding: UTF-8.

SPLIT FILE OFF.

*Diskriptive Statistik mit Normalverteilung ausgeben.
EXAMINE VARIABLES=N_TeamCogTrustScore TeamRoundsDone BY TeamID
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
  /VARIABLES=N_TeamCogTrustScore TeamRoundsDone
  /PRINT=SPEARMAN TWOTAIL NOSIG
  /MISSING=PAIRWISE.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=TeamRoundsDone N_TeamCogTrustScore MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE
  /FITLINE TOTAL=NO.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: TeamRoundsDone=col(source(s), name("TeamRoundsDone"))
  DATA: N_TeamCogTrustScore=col(source(s), name("N_TeamCogTrustScore"))
  GUIDE: axis(dim(1), label("TeamRoundsDone"))
  GUIDE: axis(dim(2), label("N_TeamCogTrustScore"))
  GUIDE: text.title(label("Einfaches Streudiagramm  von N_TeamCogTrustScore Schritt: ",
    "TeamRoundsDone"))
  ELEMENT: point(position(TeamRoundsDone*N_TeamCogTrustScore))
END GPL.



SPLIT FILE OFF.
