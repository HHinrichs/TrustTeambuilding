* Encoding: UTF-8.

* Diagrammerstellung. ----------------------- GENERAL_TRUST_SCORE.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=GENERAL_TRUST_SCORE MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: GENERAL_TRUST_SCORE=col(source(s), name("GENERAL_TRUST_SCORE"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("GENERAL_TRUST_SCORE"))
  GUIDE: text.title(label("1-D Boxplot  von GENERAL_TRUST_SCORE"))
  ELEMENT: schema(position(bin.quantile.letter(GENERAL_TRUST_SCORE)), label(id))
END GPL.



* Diagrammerstellung. ----------------------- COGNITIVE_TRUST_SCORE.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=COGNITIVE_TRUST_SCORE MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: COGNITIVE_TRUST_SCORE=col(source(s), name("COGNITIVE_TRUST_SCORE"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("COGNITIVE_TRUST_SCORE"))
  GUIDE: text.title(label("1-D Boxplot  von COGNITIVE_TRUST_SCORE"))
  ELEMENT: schema(position(bin.quantile.letter(COGNITIVE_TRUST_SCORE)), label(id))
END GPL.

* Diagrammerstellung. ----------------------- TEAM_COMMUNICATION.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=TEAM_COMMUNICATION MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: TEAM_COMMUNICATION=col(source(s), name("TEAM_COMMUNICATION"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("TEAM_COMMUNICATION"))
  GUIDE: text.title(label("1-D Boxplot  von TEAM_COMMUNICATION"))
  ELEMENT: schema(position(bin.quantile.letter(TEAM_COMMUNICATION)), label(id))
END GPL.

* Diagrammerstellung. ----------------------- TEAM_EFFECTIVENESS.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=TEAM_EFFECTIVENESS MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: TEAM_EFFECTIVENESS=col(source(s), name("TEAM_EFFECTIVENESS"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("TEAM_EFFECTIVENESS"))
  GUIDE: text.title(label("1-D Boxplot  von TEAM_EFFECTIVENESS"))
  ELEMENT: schema(position(bin.quantile.letter(TEAM_EFFECTIVENESS)), label(id))
END GPL.

* Diagrammerstellung. ----------------------- IPQ.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=IPQ MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: IPQ=col(source(s), name("IPQ"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("IPQ"))
  GUIDE: text.title(label("1-D Boxplot  von IPQ"))
  ELEMENT: schema(position(bin.quantile.letter(IPQ)), label(id))
END GPL.

* Diagrammerstellung. ----------------------- NASA_TLX.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=NASA_TLX MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: NASA_TLX=col(source(s), name("NASA_TLX"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("NASA_TLX"))
  GUIDE: text.title(label("1-D Boxplot  von NASA_TLX"))
  ELEMENT: schema(position(bin.quantile.letter(NASA_TLX)), label(id))
END GPL.

* Diagrammerstellung. ----------------------- COPRAESENZ.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=COPRAESENZ MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: COPRAESENZ=col(source(s), name("COPRAESENZ"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("COPRAESENZ"))
  GUIDE: text.title(label("1-D Boxplot  von COPRAESENZ"))
  ELEMENT: schema(position(bin.quantile.letter(COPRAESENZ)), label(id))
END GPL.
