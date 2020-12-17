* Encoding: UTF-8.
* GENERALTRUSTSCORE MIT DESKRIPTIVER STATISTIK ----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------.

DATASET ACTIVATE DataSet1.
COMPUTE GENERAL_TRUST_SCORE=Mean(Ichneigedazuanderezuakzeptieren ,
  Ichakzeptiereanderesowiesiesind ,
    MeineBeziehungenzuanderenwerdendurchVertrauenundAkzeptan ,
    IchbineinevertrauendePerson ,
    EsistbesseranderenLeuteersteinmalzuVertrauen ,
    DiemeistenMenschensindvertrauenswürdig ,
    IchfindeleichtneueFreunde ,
    Ichwürdeeingestehenmehralseinbisschenparanoidgegenüber  ,
    Ichfindeesbesseranderefürdaszuakzeptierenwassiesage ,
    IchhabevielVertrauenindieMenschendieichkenne  ,
    WennesumPersonengehtdieichkenneglaubeichdiesen ,
    IchglaubedassichmichaufdiemeistenMenschenverlassenkan ,
    FastimmerglaubeichLeutenwassiemirerzählen).
VARIABLE LABELS  GENERAL_TRUST_SCORE 'GENERAL_TRUST_SCORE'.
EXECUTE.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

DESCRIPTIVES VARIABLES=GENERAL_TRUST_SCORE
  /STATISTICS=MEAN STDDEV MIN MAX.

*COGNITIVE TRUST SCORE MIT DESKRIPTIVER STATISTIK ----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------.

DATASET ACTIVATE DataSet1.
COMPUTE COGNITIVE_TRUST_SCORE=Mean(DiesePersonGehtAnIhreArbeitMitProfessionalität ,
    AndereMitarbeiterDieMitDiesenPersonenInteragierenMüssen ,
    AngesichtsDerErfolgsbilanzDieserPersonSeheIchKeinen ,
    DieMeistenMenschenAuchDiejenigenDieKeineEndenFreunde ,
    IchKannMichDaraufVerlassenDassDiesePerosnenMeineArbeit ).
VARIABLE LABELS  COGNITIVE_TRUST_SCORE 'COGNITIVE_TRUST_SCORE'.
EXECUTE.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

DESCRIPTIVES VARIABLES=COGNITIVE_TRUST_SCORE
  /STATISTICS=MEAN STDDEV MIN MAX.

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE SEPARATE BY WiesahenihreMitspieleraus.

DESCRIPTIVES VARIABLES=COGNITIVE_TRUST_SCORE
  /STATISTICS=MEAN STDDEV MIN MAX.

*TEAM KOMMUNICATION MIT DESKRIPTIVER STATISTIK  ----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------.
*THE HIGHER THE BETTER.

DATASET ACTIVATE DataSet1.
COMPUTE TEAM_COMMUNICATION=Mean(InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem_D,
    InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem_C,
    InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem_B,
    InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem_A,
    InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem).
EXECUTE.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

 DESCRIPTIVES VARIABLES=TEAM_COMMUNICATION
 /STATISTICS=MEAN STDDEV MIN MAX.

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE SEPARATE BY WiesahenihreMitspieleraus.

DESCRIPTIVES VARIABLES=TEAM_COMMUNICATION
  /STATISTICS=MEAN STDDEV MIN MAX.

*TEAM_EFFEKTIVITÄT MIT DESKRIPTIVER STATISTIK  ----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------.

DATASET ACTIVATE DataSet1.
COMPUTE TEAM_EFFECTIVENESS=Mean(MeinTeamhateinegeringeFehlerquote,
    MeinTeamproduziertdurchgehendhochwertigeErgebnisse,
    MeinTeamhateinehoheQualität,
    MeinTeamistdurchgehendfehlerfrei).
EXECUTE.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

 DESCRIPTIVES VARIABLES=TEAM_EFFECTIVENESS
 /STATISTICS=MEAN STDDEV MIN MAX.

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE SEPARATE BY WiesahenihreMitspieleraus.

DESCRIPTIVES VARIABLES=TEAM_EFFECTIVENESS
  /STATISTICS=MEAN STDDEV MIN MAX.

*NASA TLX MIT DESKRIPTIVER STATISTIK  ----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------.

DATASET ACTIVATE DataSet1.
COMPUTE NASA_TLX=Mean(WievielgeistigeAnstrengungwarbeiderInformationsaufnahmeu,
    WievielkörperlicheAktivitätwarerforderlichz.B.ZiehenDr,
    WievielZeitdruckempfandenSiehinsichtlichderHäufigkeitode,
    WiehartmusstensiearbeitenumIhrenGradanAufgabenerfüllun,
    Wieunsicherentmutigtirritiertgestresstundverärgertver).
EXECUTE.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

 DESCRIPTIVES VARIABLES=NASA_TLX
 /STATISTICS=MEAN STDDEV MIN MAX.

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE SEPARATE BY WiesahenihreMitspieleraus.

DESCRIPTIVES VARIABLES=NASA_TLX
  /STATISTICS=MEAN STDDEV MIN MAX.
  
*PRESENCE MIT DISKRIPTIVER STATISTIK ----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------.

DATASET ACTIVATE DataSet1.
COMPUTE IPQ=Mean(IchhattenichtdasGefühlindemvirtuellenRaumzusein,
    WiesehrglichIhrErlebendervirtuellenUmgebungdemErlebene,
    IchhattedasGefühlindemvirtuellenRaumzuhandelnstattet,
    IchhattedasGefühldassdievirtuelleUmgebunghintermirwei,
    WiebewusstwarIhnendierealeWeltwährendSiesichdurchdie,
    MeineAufmerksamkeitwarvondervirtuellenWeltvölliginBann,
    DievirtuelleWelterschienmirwirklicheralsdierealeWelt,
    WierealerschienIhnendievirtuelleWelt,
    MeinerealeUmgebungwarmirnichtmehrbewusst,
    IndervomComputererzeugtenWelthatteichdenEindruckdort,
    IchfühltemichimvirtuellenRaumanwesend).
EXECUTE.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

 DESCRIPTIVES VARIABLES=IPQ
 /STATISTICS=MEAN STDDEV MIN MAX.

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE SEPARATE BY WiesahenihreMitspieleraus.

DESCRIPTIVES VARIABLES=IPQ
  /STATISTICS=MEAN STDDEV MIN MAX.
  
*COPRÄSENZ MIT DESKRIPTIVER STASTIK ----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------
----------------------------------------------------------------------.

DATASET ACTIVATE DataSet1.
COMPUTE COPRAESENZ=Mean(  
    IchwolltedieKonversationvertrautermachen,
    IchversuchteeinegewisseNähezwischenunszuerzeugen,
    IchwardaraninteressiertmitmeinenInteraktionspartnernzur,
    MeineInteraktionspartnerwarenstarkinunsererInteraktioninv,
    MeineInteraktionspartnerfandendieInteraktionenanregend,
    MeineInteraktionspartnerschienenlosgelöstwährendderInterak,
    MeineInteraktionspartnerwarenunwilligpersönlicheInformation,
    MeineInteraktionspartnermachtendenAnscheindassunsererKon,
    MeinInteraktionspartnerschufeinegewisseDistanzzwischenuns,
    MeineInteraktionspartnererstellteneinegewisseNähezwischen,
    MeineInteraktionspartnerschienengelangweiltdurchunsereKonv,
    MeineInteraktionspartnerwareninteressiertdaranmitmirzusp,
    MeineInteraktionspartnerzeigtenBegeisterungwährendersiem,
    WieintensivwardieErfahrung,
    InwiefernfühltenSiesichalswärenSieinderdargestelltenU,
    InwiefernfühltenSiesichindiedargestellteUmgebunghineinve,
    InwiefernfühltenSiesichvonderdargestelltenUmgebungumschl,
    InwiefernkonntenSieabschätzenwieIhrPartneraufdasreagie,
    WiesehrkonntenSiedieReaktiondesGegenüberabschätzen,
    WiesehrwardaseineBegegnungvonAngesichtzuAngesicht,
    WiesehrhabenSiesichmitihremPartnerimselbenRaumgefühlt,
    InwelchemAusmaßschienihrPartnerreal,
    WiewahrscheinlichistesdassSiediesesInteraktionssystemnu,
    WiegutkönntenSiejemandenkennenlernendenSienurüberdas).
EXECUTE.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

 DESCRIPTIVES VARIABLES=COPRAESENZ
 /STATISTICS=MEAN STDDEV MIN MAX.

SORT CASES  BY WiesahenihreMitspieleraus.
SPLIT FILE SEPARATE BY WiesahenihreMitspieleraus.

DESCRIPTIVES VARIABLES=COPRAESENZ
  /STATISTICS=MEAN STDDEV MIN MAX.




