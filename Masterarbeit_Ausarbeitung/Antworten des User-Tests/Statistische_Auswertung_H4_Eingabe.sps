* Encoding: UTF-8.

*Diskriptive Statistik ausgeben.
DATASET ACTIVATE DataSet1.
EXAMINE VARIABLES=RoundsDone
  /PLOT BOXPLOT NPPLOT
  /COMPARE GROUPS
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

*LeveneTest.
T-TEST GROUPS=WiesahenihreMitspieleraus(1 2)
  /MISSING=ANALYSIS
  /VARIABLES=RoundsDone
  /CRITERIA=CI(.95).

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE LAYERED BY WiesahenihreMitspieleraus.

*Diskriptive Statistik beider Konditionen.
EXAMINE VARIABLES=RoundsDone
  /PLOT BOXPLOT NPPLOT
  /COMPARE GROUPS
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

DESCRIPTIVES VARIABLES=RoundsDone
  /SAVE
  /STATISTICS=MEAN STDDEV MIN MAX.

SPLIT FILE OFF.

*Verteilungsform bestimmen.
NPAR TESTS
  /M-W= ZRoundsDone BY WiesahenihreMitspieleraus(1 2)
  /MISSING ANALYSIS.

*Mann-Whitney-U-Test.
NPAR TESTS
  /M-W= RoundsDone BY WiesahenihreMitspieleraus(1 2)
  /MISSING ANALYSIS.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=RoundsDone MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: RoundsDone=col(source(s), name("RoundsDone"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("RoundsDone"))
  GUIDE: text.title(label("1-D Boxplot  von RoundsDone"))
  ELEMENT: schema(position(bin.quantile.letter(RoundsDone)), label(id))
END GPL.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=WiesahenihreMitspieleraus MEANSE(RoundsDone, 
    2)[name="MEAN_RoundsDone" LOW="MEAN_RoundsDone_LOW" HIGH="MEAN_RoundsDone_HIGH"] MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: WiesahenihreMitspieleraus=col(source(s), name("WiesahenihreMitspieleraus"), unit.category())    
  DATA: MEAN_RoundsDone=col(source(s), name("MEAN_RoundsDone"))
  DATA: LOW=col(source(s), name("MEAN_RoundsDone_LOW"))
  DATA: HIGH=col(source(s), name("MEAN_RoundsDone_HIGH"))
  GUIDE: axis(dim(1), label("WiesahenihreMitspieleraus"))
  GUIDE: axis(dim(2), label("Mittelwert RoundsDone"))
  GUIDE: text.title(label("Einfache Balken Mittelwert  von RoundsDone Schritt: ",
    "WiesahenihreMitspieleraus"))
  GUIDE: text.footnote(label("Fehlerbalken: 95% CI"))
  GUIDE: text.subfootnote(label("Fehlerbalken: +/- 1 SE"))
  SCALE: linear(dim(2), min(1))
  ELEMENT: interval(position(WiesahenihreMitspieleraus*MEAN_RoundsDone), 
    shape.interior(shape.square))
  ELEMENT: interval(position(region.spread.range(WiesahenihreMitspieleraus*(LOW+HIGH))), 
    shape.interior(shape.ibeam))
END GPL.
