﻿* Encoding: UTF-8.
DATASET ACTIVATE DataSet1.
SPLIT FILE OFF.

DATASET ACTIVATE DataSet1.
CORRELATIONS
  /VARIABLES=RoundsDone COGNITIVE_TRUST_SCORE
  /PRINT=TWOTAIL NOSIG
  /MISSING=PAIRWISE.

DATASET ACTIVATE DataSet1.
NONPAR CORR
  /VARIABLES=RoundsDone COGNITIVE_TRUST_SCORE
  /PRINT=SPEARMAN TWOTAIL NOSIG
  /MISSING=PAIRWISE.

SORT CASES  BY Merkmal.
SPLIT FILE SEPARATE BY Merkmal.

DATASET ACTIVATE DataSet1.
CORRELATIONS
  /VARIABLES=RoundsDone COGNITIVE_TRUST_SCORE
  /PRINT=TWOTAIL NOSIG
  /MISSING=PAIRWISE.

REGRESSION
  /MISSING LISTWISE
  /STATISTICS COEFF OUTS R ANOVA
  /CRITERIA=PIN(.05) POUT(.10)
  /NOORIGIN 
  /DEPENDENT RoundsDone
  /METHOD=ENTER COGNITIVE_TRUST_SCORE.


*-------------------ZUSÄTZLICHE KORRETOR.

DATASET ACTIVATE DataSet2.
* Diagrammerstellung.
GGRAPH
  /GRAPHDATASET NAME="graphdataset" VARIABLES=COGNITIVE_TRUST_SCORE RoundsDone MISSING=LISTWISE 
    REPORTMISSING=NO
  /GRAPHSPEC SOURCE=INLINE
  /FITLINE TOTAL=NO.
BEGIN GPL
  SOURCE: s=userSource(id("graphdataset"))
  DATA: COGNITIVE_TRUST_SCORE=col(source(s), name("COGNITIVE_TRUST_SCORE"))
  DATA: RoundsDone=col(source(s), name("RoundsDone"))
  GUIDE: axis(dim(1), label("COGNITIVE_TRUST_SCORE"))
  GUIDE: axis(dim(2), label("RoundsDone"))
  GUIDE: text.title(label("Einfaches Streudiagramm  von RoundsDone Schritt: COGNITIVE_TRUST_SCORE"))    
  ELEMENT: point(position(COGNITIVE_TRUST_SCORE*RoundsDone))
END GPL.



CORRELATIONS
  /VARIABLES=RoundsDone COGNITIVE_TRUST_SCORE
  /PRINT=TWOTAIL NOSIG
  /MISSING=PAIRWISE.

REGRESSION
  /MISSING LISTWISE
  /STATISTICS COEFF OUTS R ANOVA
  /CRITERIA=PIN(.05) POUT(.10)
  /NOORIGIN 
  /DEPENDENT RoundsDone
  /METHOD=ENTER COGNITIVE_TRUST_SCORE.

