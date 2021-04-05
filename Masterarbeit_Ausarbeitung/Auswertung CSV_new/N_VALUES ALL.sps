* Encoding: UTF-8.

DATASET ACTIVATE DataSet1.
COMPUTE N_GenTrust=MEAN(Ichneigedazuanderezuakzeptieren to 
    FastimmerglaubeichLeutenwassiemirerzählen).
EXECUTE.

COMPUTE N_CogTrust=MEAN(DiesePersonGehtAnIhreArbeitMitProfessionalität to 
    WennDieMenschenMehrÜberDiesePersonUndIhrenHintergrundWüssten).
EXECUTE.

COMPUTE N_TeamCommunication=MEAN(InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem_D to 
    InwelchemUmfangwardieKommunikationzwischenIhnenundIhrem).
EXECUTE.

COMPUTE N_TeamEffectiveness=MEAN(MeinTeamhateinegeringeFehlerquote to 
    MeinTeammussihreArbeitsqualitätverbessern).
EXECUTE.

COMPUTE N_NTLX=MEAN(WievielgeistigeAnstrengungwarbeiderInformationsaufnahmeu to 
    Wieunsicherentmutigtirritiertgestresstundverärgertver).
EXECUTE.

COMPUTE N_IPQ=MEAN(IchhattenichtdasGefühlindemvirtuellenRaumzusein to 
    IchachtetenochaufdierealeUmgebung).
EXECUTE.

COMPUTE N_SelbstCoPresence=MEAN(IchwolltekeineengereBeziehungmitmeinenInteraktionspartner to 
    IchwardaraninteressiertmitmeinenInteraktionspartnernzur).
EXECUTE.

COMPUTE N_OthersCoPresence=MEAN(MeineInteraktionspartnerwarenstarkinunsererInteraktioninv to 
    MeineInteraktionspartnerzeigtenBegeisterungwährendersiem).
EXECUTE.

COMPUTE N_Telepresence=MEAN(WieinvolvierendwardasErgebnis to 
    InwiefernfühltenSiesichvonderdargestelltenUmgebungumschl).
EXECUTE.

COMPUTE N_SocialPresence=MEAN(InwiefernkonntenSieabschätzenwieIhrPartneraufdasreagie to 
    WiegutkönntenSiejemandenkennenlernendenSienurüberdas).
EXECUTE.
