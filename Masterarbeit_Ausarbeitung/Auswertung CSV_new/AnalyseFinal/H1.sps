* Encoding: UTF-8.

*Normalverteilung.
EXAMINE VARIABLES=N_CogTrust
  /PLOT BOXPLOT NPPLOT
  /COMPARE GROUPS
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

EXAMINE VARIABLES=N_CogTrust BY WiesahenihreMitspieleraus
  /PLOT BOXPLOT NPPLOT
  /COMPARE GROUPS
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

*Verteilungsformen bestimmen.
NPAR TESTS
  /K-S= ZN_CogTrust BY WiesahenihreMitspieleraus(1 2)
  /MISSING ANALYSIS.

*TTest.
NPAR TESTS
  /M-W= N_CogTrust BY WiesahenihreMitspieleraus(1 2)
  /STATISTICS=DESCRIPTIVES QUARTILES
  /MISSING ANALYSIS
  /METHOD=EXACT TIMER(5).

* Boxplot.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=N_CogTrust MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: N_CogTrust=col(source(s), name("N_CogTrust"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("Kognitives Vertrauen"))
  GUIDE: text.title(label("1-D Boxplot  von Kognitives Vertrauen"))
  ELEMENT: schema(position(bin.quantile.letter(N_CogTrust)), label(id))
END GPL.

* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=WiesahenihreMitspieleraus MEANSE(N_CogTrust, 
    1)[name="MEAN_N_CogTrust" LOW="MEAN_N_CogTrust_LOW" HIGH="MEAN_N_CogTrust_HIGH"] MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: WiesahenihreMitspieleraus=col(source(s), name("WiesahenihreMitspieleraus"), unit.category())    
  DATA: MEAN_N_CogTrust=col(source(s), name("MEAN_N_CogTrust"))
  DATA: LOW=col(source(s), name("MEAN_N_CogTrust_LOW"))
  DATA: HIGH=col(source(s), name("MEAN_N_CogTrust_HIGH"))
  GUIDE: axis(dim(1), label("WiesahenihreMitspieleraus"))
  GUIDE: axis(dim(2), label("Skalenwerte Kognitives Vertrauen"))
  GUIDE: text.title(label("Einfache Balken Mittelwert  von Kognitives Vertrauen Schritt: ",
    "WiesahenihreMitspieleraus"))
  GUIDE: text.footnote(label("Fehlerbalken: 95% CI"))
  GUIDE: text.subfootnote(label("Fehlerbalken: +/- 1 SE"))
  SCALE: linear(dim(2), min(1))
  ELEMENT: interval(position(WiesahenihreMitspieleraus*MEAN_N_CogTrust), 
    shape.interior(shape.square))
  ELEMENT: interval(position(region.spread.range(WiesahenihreMitspieleraus*(LOW+HIGH))), 
    shape.interior(shape.ibeam))
END GPL.
