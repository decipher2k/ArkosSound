from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM
from numpy import array
from keras.models import load_model
import sys

import warnings
warnings.filterwarnings("ignore")

# load model from single file
model = load_model('lstm_model.h5')
print(len(sys.argv))

X=[90,92,88,90,93,88,81,-1000]

# total arguments
n = len(sys.argv)
if(n==8):
	X[0]=int(sys.argv[1])
	X[1]=int(sys.argv[2])
	X[2]=int(sys.argv[3])
	X[3]=int(sys.argv[4])
	X[4]=int(sys.argv[5])
	X[5]=int(sys.argv[6])
	X[6]=int(sys.argv[7])

print("RDY")

while 1==1:
    # make predictions
    Xinp=array(X)
    Xinp=Xinp.reshape(-1,8,1)
    yhat = model.predict(Xinp, verbose=0)
    print(int(yhat[0]))

    for i in  range(0,6):        
        X[i]=X[i+1]
    X[6]=int(yhat)
    
    inp=input()
    if(inp=='a'):
    	X[7]=-1000
    else:
     	X[7]=1000
