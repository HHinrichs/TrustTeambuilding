* Encoding: UTF-8.

DATASET ACTIVATE DataSet1.
COMPUTE General_Trust=Mean(Ichneigedazuanderezuakzeptieren to 
    FastimmerglaubeichLeutenwassiemirerzählen).
VARIABLE LABELS  General_Trust 'General_Trust_Score'.
EXECUTE.
