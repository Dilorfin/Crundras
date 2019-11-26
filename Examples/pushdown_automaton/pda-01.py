class Stack:
    def __init__(self):
         self.items = []

    def isEmpty(self):
         return self.items == []

    def push(self, item):
         self.items.append(item)

    def pop(self):
         return self.items.pop()

    def print(self):
        print('STACK: {0} '.format(' '.join(self.items)))
        return True


#state-transition_function={}
stf_Automata={'init':0,     \
              'final':{3},  \
              'transition':{(0,'ɛ','ɛ'):(1,'ɛ'),                \
                            (1,'a','ɛ'):(1,'a'), (1,'b','a'):(2,'ɛ'),   \
                            (2,'b','a'):(2,'ɛ'), (2,'ɛ','Z'):(3,'ɛ')  \
            }
}

tableOfSymb='aaacbbb'

class Pda:
    def __init__(self,tableOfSymbols,stack,stf):
        self.stack=stack 
        if self.stack.isEmpty():
            self.stack.push('Z')
        self.stf=stf
        self.tblSymb=tableOfSymbols
        self.state=stf['init']
        self.finalStates=stf['final']
        self.isRight = True
        self.stopMachine = False

    def configurationPrint(self):
        print('\nConfiguration:\t\t STATE: {0}\tiput: {1}\tSTACK: {2} '.format(self.state,self.tblSymb,' '.join(self.stack.items)))
        return True

    def ahead(self):
        if len(self.tblSymb)>0:
            res = self.tblSymb[0]
        else:
            res = 'ɛ'
        return res

    def topInStack(self):
        if len(self.stack.items)>0:
            res = self.stack.items[-1]
        else:
            res = ''
        return res

    def pop(self):
        symb = self.stack.pop()
        return symb

    def push(self,symb):
        self.stack.push(symb)
        return self.stack

    def fail(self,data):
        print('fail(): ',data)
        self.configurationPrint()
        self.stopMachine = True
##        exit()
        return False
    
    def nextState(self):
        fromTopStack=''
        if self.isRight:
##            self.configurationPrint()
            keys=list(stf_Automata['transition'].keys())
            symb=self.ahead()
            top=self.topInStack()
            fOk=[key for key in keys if key == (self.state,symb,top)]
            fSymb=[key for key in keys if key == (self.state,symb,'ɛ')]
            fTop=[key for key in keys if key == (self.state,'ɛ',top)]
            fNo=[key for key in keys if key == (self.state,'ɛ','ɛ')]
            if fOk != []:
                to=stf_Automata['transition'][(self.state,symb,top)]
                self.tblSymb = self.tblSymb[1:len(self.tblSymb)]
                fromTopStack=self.stack.pop()
            elif fSymb != []:
                to=stf_Automata['transition'][(self.state,symb,'ɛ')]
                self.tblSymb = self.tblSymb[1:len(self.tblSymb)]
            elif fTop != []:
                to=stf_Automata['transition'][(self.state,'ɛ',top)]
                fromTopStack=self.stack.pop()
            elif fNo != []:
                to=stf_Automata['transition'][(self.state,'ɛ','ɛ')]
            else:
                self.isRight=self.fail({'issue with (state,input symb, top stack)':(self.state,symb,top)})
            if self.isRight and not self.stopMachine:
                print('\n \n\t\t\t nextState: \n')
                print('(state,symb,top),-->,(newState,push)')
                print((self.state,symb,top),'-->',to)
                self.state=to[0]
##                self.configurationPrint()
                if to[1] != 'ɛ':
                    self.stack.push(to[1])
##                self.configurationPrint()
                if fromTopStack=='Z':
                    self.stopMachine = True
            print('\n isRight={0} \t stopMachine={1}'.format(self.isRight,self.stopMachine))
            self.configurationPrint()
        return self.isRight
    
    
    def run(self):
        while self.isRight and not self.stopMachine:
            if not self.isRight or self.stopMachine:
                print('break')
                break
            if self.isRight and not self.stopMachine:
                self.isRight = self.nextState()
        B =  (self.isRight and self.stopMachine and self.state in self.finalStates and  len(self.tblSymb)==0 and self.stack.isEmpty())
        print('\nisSuccess = {}'.format(B))     
        return True
        
stack2=Stack()
tableOfSymb='ab'           
pda=Pda(tableOfSymb,stack2,stf_Automata)
##pda.configurationPrint()
##pda.run()
##pda.nextState()
##pda.nextState()
##pda.nextState()
##pda.nextState()
##pda.nextState()
##pda.nextState()
##pda.nextState()
##pda.nextState()
##pda.nextState()
##pda.nextState()

pda.run()


















