'''
    Several stats
'''

import numpy as np
from collections import defaultdict

names = ["john","mary","peter","mathew","angel","simon","luke","manuel","andrew","philip"]
ids = np.arange(0,10,1)
users = defaultdict(dict)
for id in ids:
    users["id"] = id
    #users["id"]["name"] = names[id]

print(users)
