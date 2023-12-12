#!/usr/bin/env python
# coding: utf-8

# In[3]:


# In[4]:

from tqdm.keras import TqdmCallback
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM
from numpy import array
from keras.models import load_model
import keras 
import tensorflow as tf 
from tensorflow.python.keras import backend as K
config = tf.compat.v1.ConfigProto( device_count = {'GPU': 1 } ) 
sess = tf.compat.v1.Session(config=config) 
K.set_session(sess)

# In[16]:
class NBatchLogger():
    def __init__(self, display):
        self.seen = 0
        self.display = display

    def on_batch_end(self, batch, logs={}):
        self.seen += logs.get('size', 0)
        if self.seen % self.display == 0:
            metrics_log = ''
            for k in self.params['metrics']:
                if k in logs:
                    val = logs[k]
                    if abs(val) > 1e-3:
                        metrics_log += ' - %s: %.4f' % (k, val)
                    else:
                        metrics_log += ' - %s: %.4e' % (k, val)
            print('{}/{} ... {}'.format(self.seen,
                                        self.params['samples'],
                                        metrics_log))


# return training data
def get_train():
    X=[]
    xTmp=[]
    count=0;
    lineCount=0
    with open('outX.txt', 'r') as file:
        line = file.readline()
        while line:
            xTmp.append(float(line.strip()))
            count=count+1
            line = file.readline()
            if(count % 8 == 0):
                count=0
                X.append(xTmp)
                lineCount=lineCount+1
                #print(xTmp)
                xTmp=[]
    
    y=[]
    yTmp=[]
    lineCount=0
    with open('outY.txt', 'r') as file:
        line = file.readline()
        while line:
            y.append(float(line.strip()))
            lineCount=lineCount+1
            line = file.readline()
    
    y=array(y)
    X=array(X)
    X=X.reshape(int(X.size/8),8,1)
    y=y.reshape(y.size,1)
    return X, y
    
# define model
model = Sequential()
model.add(LSTM(32, input_shape=(8,1), return_sequences=True))
model.add(LSTM(32, input_shape=(8,1), return_sequences=True))
model.add(LSTM(32, input_shape=(8,1)))
model.add(Dense(1, activation='relu'))
# compile model

model.compile(loss=tf.keras.losses.mean_squared_error, optimizer = tf.keras.optimizers.legacy.Adam(lr=0.0003, decay=1e-6),metrics=['accuracy'])
# fit model
X,y = get_train()
out_batch = NBatchLogger(display=1000)
model.fit(X, y, epochs=1250, batch_size=32, validation_split=0.2, shuffle=True, callbacks=[TqdmCallback(verbose=2)])
# save model to single file
model.save('lstm_model.h5')
 
# snip...
# later, perhaps run from another script



