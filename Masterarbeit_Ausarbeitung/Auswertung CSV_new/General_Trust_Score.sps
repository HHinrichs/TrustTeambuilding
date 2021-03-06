﻿* Encoding: UTF-8.

* DATEN WINSORIEREN ???? 
https://statistikguru.de/spss/produkt-moment-korrelation/mit-ausreissern-umgehen-2.html .

*CHRONBACHS ALPHA FÜR GENERAL TRUST SCORE
EINIGE ITEMS WURDEN NUN WEGGELASSEN UM DEN FRAGEBOGEN IN SICH KONSESTENZ ZU GESTALTEN. DIESE NENNEN.


*All Over Efficiency ------------------------------------------------------------------------------.
DATASET ACTIVATE DataSet1.
COMPUTE AlloverEfficiency=Mean(Round1Efficiency to Round15Efficiency).
VARIABLE LABELS AlloverEfficiency 'AlloverEfficiency'.
EXECUTE.

* GENERAL TRUST CORRELATION TO COGNITIVE TRUST ------------------------------------------------------------------------------.

* KEINE AUßREIßER COGNITIVE TRUST.
* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=Cognitive_Trust MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: Cognitive_Trust=col(source(s), name("Cognitive_Trust"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  GUIDE: axis(dim(2), label("Cognitive_Trust_Score"))
  GUIDE: text.title(label("Einfacher Boxplot  von Cognitive_Trust_Score"))
  ELEMENT: schema(position(bin.quantile.letter(1*Cognitive_Trust)), label(id))
END GPL.


* KEINE AUßREIßER GENERAL TRUST.
* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=General_Trust MISSING=LISTWISE REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: General_Trust=col(source(s), name("General_Trust"))
  DATA: id=col(source(s), name("$CASENUM"), unit.category())
  GUIDE: axis(dim(2), label("General_Trust_Score"))
  GUIDE: text.title(label("Einfacher Boxplot  von General_Trust_Score"))
  ELEMENT: schema(position(bin.quantile.letter(1*General_Trust)), label(id))
END GPL.

* AUF LINEARITÄT MITTELS STREUDIAGRAMM ÜBERPRÜFEN -> AM BESTEN FÜR MICH POSITIVER ZUSAMMENHANG
* Diagrammerstellung. 
GGRAPH 
  /GRAPHDATASET NAME="graphdataset" VARIABLES=Cognitive_Trust General_Trust MISSING=LISTWISE 
    REPORTMISSING=NO 
  /GRAPHSPEC SOURCE=INLINE 
  /FITLINE TOTAL=YES. 
BEGIN GPL 
  SOURCE: s=userSource(id("graphdataset")) 
  DATA: Cognitive_Trust=col(source(s), name("Cognitive_Trust")) 
  DATA: General_Trust=col(source(s), name("General_Trust")) 
  GUIDE: axis(dim(1), label("Cognitive_Trust_Score")) 
  GUIDE: axis(dim(2), label("General_Trust_Score")) 
  GUIDE: text.title(label("Einfache Streuung mit Anpassungslinie  von General_Trust_Score ", 
    "Schritt: Cognitive_Trust_Score")) 
  ELEMENT: point(position(Cognitive_Trust*General_Trust)) 
END GPL.

*MITTELWERTE SOLLTEN NUN AUF BIVARIATÄT ÜBERPRÜFT WERDEN
GEHT NICHT MIT SPSS, ALSO ZENTRALEN GRENZWERTSATZ ANWENDEN DER BESAGT, DASS DAVON AUSGEGANGEN WIRD, 
DASS DIE VERTEILUNG VON MITTELWERTEN AUS STICHPROBEN n>=30 NORMALVERWEILT SIND WENN DIESE AUS EINER BELIEBIG VERTEILTEN GRUNDGESAMTHEIT ENTNOMMEN WURDEN.

* NORMALE CORRELATION FÜR N>= 30 STICHPROBEN NACH PEARSON.
CORRELATIONS
  /VARIABLES=General_Trust Cognitive_Trust
  /PRINT=TWOTAIL NOSIG
  /MISSING=PAIRWISE.

* BOOTSTRAP CORRELATION NACH PEARSON FÜR N<30 STICHPROBEN.
BOOTSTRAP
  /SAMPLING METHOD=SIMPLE
  /VARIABLES INPUT=General_Trust Cognitive_Trust 
  /CRITERIA CILEVEL=95 CITYPE=BCA  NSAMPLES=1000
  /MISSING USERMISSING=EXCLUDE.
CORRELATIONS
  /VARIABLES=General_Trust Cognitive_Trust
  /PRINT=TWOTAIL NOSIG
  /MISSING=PAIRWISE.

*FAKTORENANALSE FÜR GENERAL TRUST SCALE. ES WURDEN 3 FAKTOREN ENTDECKT
EINIGE ITEMS WURDEN NUN WEGGELASSEN UM DEN FRAGEBOGEN IN SICH KONSESTENZ ZU GESTALTEN. DIESE NENNEN
https://www.youtube.com/watch?v=S9BEit71OvI&ab_channel=DanielaKeller-StatistikundBeratun .
DATASET ACTIVATE DataSet1.
FACTOR
  /VARIABLES MeineBeziehungenzuanderenwerdendurchVertrauenundAkzeptan IchbineinevertrauendePerson 
    EsistbesseranderenLeuteersteinmalzuVertrauen EsistbesserFremdenzumisstrauenbismansiebesserkennt 
    IchfindeleichtneueFreunde IchhabeetwasSchwierigkeitenLeutenzuvertrauen 
    IchhabekeinVertraueninanderePersonen MeineErfahrungenhabenmirzeigenmirdassesbesseristande 
    WennesumPersonengehtdieichkenneglaubeichdiesen FastimmerglaubeichLeutenwassiemirerzählen
  /MISSING LISTWISE 
  /ANALYSIS MeineBeziehungenzuanderenwerdendurchVertrauenundAkzeptan IchbineinevertrauendePerson 
    EsistbesseranderenLeuteersteinmalzuVertrauen EsistbesserFremdenzumisstrauenbismansiebesserkennt 
    IchfindeleichtneueFreunde IchhabeetwasSchwierigkeitenLeutenzuvertrauen 
    IchhabekeinVertraueninanderePersonen MeineErfahrungenhabenmirzeigenmirdassesbesseristande 
    WennesumPersonengehtdieichkenneglaubeichdiesen FastimmerglaubeichLeutenwassiemirerzählen
  /PRINT UNIVARIATE INITIAL CORRELATION SIG KMO AIC EXTRACTION ROTATION
  /FORMAT BLANK(0.3)
  /PLOT EIGEN
  /CRITERIA MINEIGEN(1) ITERATE(25)
  /EXTRACTION PC
  /CRITERIA ITERATE(25)
  /ROTATION VARIMAX
  /METHOD=CORRELATION.

*T-TEST FÜR UNABHGÄNGIGE STICHPROBEN BEI COGNITIVE TRUST.. ES WIRD KEINE SIGNIFIKANZ FESTGESTELLT, COGNITIVE TRUST SCORE ÄNDERT SICH NICHT SIGNIFIKANT, OB IK ODER NON IK AVATAR GENUTZT WIRD
ES WURDE WINSORIZING GENUTZT.

T-TEST GROUPS=WiesahenihreMitspieleraus(2 1)
  /MISSING=ANALYSIS
  /VARIABLES=Cognitive_Trust_Winsorizing
  /CRITERIA=CI(.95).

* NORMALVERTEILUNG BINOMINALVERTEILUNG
