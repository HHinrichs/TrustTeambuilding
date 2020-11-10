* Encoding: UTF-8.

* GeneralTrustScore ------------------------------------------------------------------------------.
DATASET ACTIVATE DataSet1.
COMPUTE General_Trust=Mean(Ichneigedazuanderezuakzeptieren to 
    FastimmerglaubeichLeutenwassiemirerzählen).
VARIABLE LABELS  General_Trust 'General_Trust_Score'.
EXECUTE.

*GeneralTrustScore ------------------------------------------------------------------------------.
COMPUTE Cognitive_Trust=Mean(IchwarmirsicherdassdieanderenTeilnehmerintelligenteEnt to 
    IchhattesorgendassdieanderenTeilnehmeretwasfalschmache).
VARIABLE LABELS  Cognitive_Trust 'Cognitive_Trust_Score'.
EXECUTE.

*All Over Efficiency ------------------------------------------------------------------------------.
DATASET ACTIVATE DataSet1.
COMPUTE AlloverEfficiency=Mean(Round1Efficiency to Round15Efficiency).
VARIABLE LABELS AlloverEfficiency 'AlloverEfficiency'.
EXECUTE.

*Team Effectiveness --------------------------------------------------------------------------.
DATASET ACTIVATE DataSet1.
COMPUTE TEAM_EFFECTIVENESS=Mean(MeinTeamhateinegeringeFehlerquote to 
    MeinTeammussihreArbeitsqualitätverbessern).
EXECUTE.

DESCRIPTIVES VARIABLES=TEAM_EFFECTIVENESS
  /STATISTICS=MEAN STDDEV MIN MAX.

*Team Communication Quality --------------------------------------------------------------------------.
*THE HIGHER THE BETTER.
DATASET ACTIVATE DataSet1.
COMPUTE TEAM_COMMUNICATION=Mean(InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem_D to 
    InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem).
EXECUTE.

DESCRIPTIVES VARIABLES=TEAM_COMMUNICATION
 /STATISTICS=MEAN STDDEV MIN MAX.


*PRÄSENZ ----------------------------------------------------------------------------------------------------
Nowak and Biocca, 2003.
COMPUTE PRÄSENZ=Mean(IchwolltekeineengereBeziehungmitmeinenInteraktionspartner to 
    WiegutkönntenSiejemandenkennenlernendenSienurüberdas).
EXECUTE.

 DESCRIPTIVES VARIABLES=PRÄSENZ
 /STATISTICS=MEAN STDDEV MIN MAX.


*IPQ.EMMERSION -----------------------------------------------------------------------
*THE HIGHER THE MORE EMMERSIVE.
DATASET ACTIVATE DataSet1.
COMPUTE IPQ=Mean(IchhattenichtdasGefühlindemvirtuellenRaumzusein to 
    IchachtetenochaufdierealeUmgebung).
EXECUTE.

 DESCRIPTIVES VARIABLES=IPQ
 /STATISTICS=MEAN STDDEV MIN MAX.

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
