* Encoding: UTF-8.
*Altersdiagramm.
DATASET ACTIVATE DataSet1.
FREQUENCIES VARIABLES=Alter
  /NTILES=4
  /STATISTICS=MEAN
  /BARCHART FREQ
  /ORDER=ANALYSIS.

* Boxplot des Alters.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=Alter MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: Alter=col(source(s), name("Alter"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  COORD: rect(dim(1), transpose())
  GUIDE: axis(dim(1), label("Alter"))
  GUIDE: text.title(label("1-D Boxplot  von Alter"))
  ELEMENT: schema(position(bin.quantile.letter(Alter)), label(id))
END GPL.

*Biologisches Geschlecht Kreisdiagramm.
FREQUENCIES VARIABLES=BiologischesGeschlecht
  /NTILES=4
  /STATISTICS=MEAN
  /PIECHART FREQ
  /ORDER=ANALYSIS.

*Höchster Bildungsstand Kreisdiagramm.
FREQUENCIES VARIABLES=Bildungsstand
  /NTILES=4
  /STATISTICS=MEAN
  /PIECHART FREQ
  /ORDER=ANALYSIS.

*Teilnehmer VR Vorerfahrung.
FREQUENCIES VARIABLES=VRErfahrung
  /NTILES=4
  /STATISTICS=MEAN
  /PIECHART FREQ
  /ORDER=ANALYSIS.

*Teilnehmer VR-Studien Erfahrung.
FREQUENCIES VARIABLES=Vorexperimente
  /NTILES=4
  /STATISTICS=MEAN
  /PIECHART FREQ
  /ORDER=ANALYSIS.

*Teilnehmer Internet Ausmaß.
FREQUENCIES VARIABLES=InternetAußmaß
  /NTILES=4
  /STATISTICS=MEAN
  /BARCHART FREQ
  /ORDER=ANALYSIS.

*Teilnehmer Videospiele Ausmaß.
FREQUENCIES VARIABLES=VideospieleAußmaß
  /NTILES=4
  /STATISTICS=MEAN
  /BARCHART FREQ
  /ORDER=ANALYSIS.
