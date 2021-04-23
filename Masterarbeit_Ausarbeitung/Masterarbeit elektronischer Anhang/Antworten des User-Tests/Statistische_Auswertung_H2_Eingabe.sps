* Encoding: UTF-8.
*Diskriptive Statistik.
EXAMINE VARIABLES=N_GenTrust N_CogTrust BY WiesahenihreMitspieleraus
  /PLOT BOXPLOT NPPLOT
  /COMPARE GROUPS
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE LAYERED BY WiesahenihreMitspieleraus.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=N_GenTrust N_CogTrust MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE
  /FITLINE TOTAL=NO.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: N_GenTrust=col(source(s), name("N_GenTrust"))
  DATA: N_CogTrust=col(source(s), name("N_CogTrust"))
  GUIDE: axis(dim(1), label("Generelles Vertrauen"))
  GUIDE: axis(dim(2), label("Kognitives Vertrauen"))
  GUIDE: text.title(label("Einfaches Streudiagramm  von Kognitives Vertrauen Schritt: Generelles ",
    "Vertrauen"))
  ELEMENT: point(position(N_GenTrust*N_CogTrust))
END GPL.

SPLIT FILE OFF.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=N_GenTrust MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: N_GenTrust=col(source(s), name("N_GenTrust"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("Generelles Vertrauen"))
  GUIDE: text.title(label("1-D Boxplot  von Generelles Vertrauen"))
  ELEMENT: schema(position(bin.quantile.letter(N_GenTrust)), label(id))
END GPL.


SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE LAYERED BY WiesahenihreMitspieleraus.

*Spearman-Korrelation berechnen.
NONPAR CORR
  /VARIABLES=N_GenTrust N_CogTrust
  /PRINT=SPEARMAN TWOTAIL NOSIG
  /MISSING=PAIRWISE.

SPLIT FILE OFF.

