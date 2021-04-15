* Encoding: UTF-8.

DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

*Test auf Normalverteilung und Boxplots und Deskriptive Statistik.
EXAMINE VARIABLES=N_TeamCommunication N_TeamEffectiveness N_NTLX N_IPQ N_SelbstCoPresence 
    N_OthersCoPresence N_Telepresence N_SocialPresence
  /PLOT BOXPLOT NPPLOT
  /COMPARE GROUPS
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

*Deskriptive Statistik der subjektiven Daten für die einzelnen Konditionen.
EXAMINE VARIABLES=N_TeamCommunication N_TeamEffectiveness N_NTLX N_IPQ N_SelbstCoPresence 
    N_OthersCoPresence N_Telepresence N_SocialPresence BY WiesahenihreMitspieleraus
  /PLOT NONE
  /STATISTICS DESCRIPTIVES
  /CINTERVAL 95
  /MISSING LISTWISE
  /NOTOTAL.

*Mann-Whitney-U-Tests.
NPAR TESTS
  /M-W= N_SelbstCoPresence N_Telepresence N_SocialPresence N_TeamCommunication N_TeamEffectiveness 
    BY WiesahenihreMitspieleraus(1 2)
  /MISSING ANALYSIS.

*T-Tests.
T-TEST GROUPS=WiesahenihreMitspieleraus(1 2)
  /MISSING=ANALYSIS
  /VARIABLES=N_IPQ N_NTLX N_OthersCoPresence
  /CRITERIA=CI(.95).
