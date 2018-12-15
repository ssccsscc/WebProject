from anytree import *
import os
import random


class Battlefield:
    class Tile:
        # 0 - unknown # 1 - missed
        # 2 - damaged # 3 - killed
        # 4 - aim
        def __init__(self):
            self.state = 0
            #self.coordinats = (, d[y])

        def missed(self):
            self.state = 1

        def damaged(self):
            self.state = 2

        def killed(self):
            self.state = 3

        def Aim(self):
            self.state = 4

        def __str__(self):
            d = {0: 'O', 1: " ", 2: "X", 3: 'X', 4: '?'}
            return d[self.state]


    def __init__(self):
        self.Matrix = []
        for i in range(0, 10):
            self.Matrix.append([])
            for j in range(0, 10):
                self.Matrix[i].append(self.Tile())
        self.Backlog = []
        self.Ships = [1,1,1,1,2,2,2,3,3,4]
        self.DamageTree = None
        self.AI = ''
        self.DamageList = []
        self.LastShot = []
        self.Tactic = []
        self.DefaultTactic()

    def __str__(self):
        fin = ''
        for i in range(0, 10):
            fin += str(i) + '|'
            for j in range(0, 10):
                fin += '  ' + str(self.Matrix[i][j])
            fin += '\n'
        fin += '-+------------------------------\n'
        fin += ' |  0  1  2  3  4  5  6  7  8  9\n'
        #fin += ' |  A  B  C  D  E  F  G  H  I  J\n'
        return fin

    def Around(self, i):
        # Координаты воокруг клетки i
        return [(i[0] + 1, i[1] - 1), (i[0] + 1, i[1]), (i[0] + 1, i[1] + 1), (i[0], i[1] + 1),
                (i[0], i[1] - 1), (i[0] - 1, i[1] - 1), (i[0] - 1, i[1]), (i[0] - 1, i[1] + 1)]
				
    def AroundCrest(self, i):
        # Координаты по кресту от клетки i
        return [(i[0] + 1, i[1]), (i[0] - 1, i[1]), (i[0], i[1] + 1), (i[0], i[1] - 1)]

    def ExtendAroundCrest(self, i):
        # Координаты по кресту + 1 от клетки i
        return [(i[0] + 1, i[1]), (i[0] - 1, i[1]), (i[0], i[1] + 1), (i[0], i[1] - 1), (i[0] + 2, i[1]), (i[0] - 2, i[1]), (i[0], i[1] + 2), (i[0], i[1] - 2)]


    def ClearTile(self, list):
        # Список чистых клеток по листу
        if type(list) is tuple:
            return  self.isClear(list[0], list[1])
        fin = []
        for j in list:
            if self.isClear(j[0], j[1]):
                fin.append((j[0], j[1]))
        return fin


    def Backup(self):
        # delete this
        self.Backlog.append(self.Matrix.copy(), self.Tactic.copy())

    def Restore(self):
        # delete this
        self.Matrix = self.Backlog[0].copy()
        self.Tactic = self.Backlog[1].copy()

    def Response(self, x, y, step=0, stepy=0):
        # Ожидание ответа
        b = True
        while b:
            k = (input('# 0 - missed\n# 1 - damaged\n# 2 - killed\n'))

            if k in {'0', '`', 'ё'}:
                b = False
                self.Matrix[x + step][y + stepy].missed()
                self.Missed()
            elif k == '1':
                b = False
                self.Matrix[x + step][y + stepy].damaged()
                self.Damaged(x + step, y + stepy)
            elif k == '2':
                b = False
                self.Matrix[x + step][y + stepy].killed()
                self.Killed()
            else:
                continue



    def Volley(self, x, y):
        # delete this
        coordinats = [x, y]#self.Decide()
        self.LastShot = coordinats
        self.Response(coordinats[0], coordinats[1])

    def VolleyD(self):
        # Выбор координат и срельба
        coordinats = self.Decide()
        self.Matrix[coordinats[0]][coordinats[1]].state = 4
        #d = {1:'А', 2:'Б', 3:"В", 4:"Г", 5:"Д", 6:"Е", 7:"Ж", 8:"З", 9:"И", 10:"К"}
        self.LastShot = coordinats
        print(battlefild)
        print("{0} {1}".format(coordinats[0], coordinats[1]))
        self.Response(coordinats[0], coordinats[1])

    def Damaged(self, i, j):
        # Действия после дамага клетки
        self.DamageList.append(self.LastShot)
        if len(self.DamageList) == max(self.Ships):
            self.Killed()
            return 0
        # Если эта первая задамаженная клетка, то идет построение дерева всех возможных вариантов стрельбы
        if self.DamageTree == None:
            self.DamageTree = Node(name='root', i=i, j=j)
            for k in [[i+1, j, 'right'], [i, j+1, 'down'], [i-1, j, 'left'], [i, j-1, 'up']]:
                if self.isClear(k[0], k[1]):
                    Node(name=k[2], i=k[0], j=k[1], parent=self.DamageTree)
            if max(self.Ships) > 2:
                for h in self.DamageTree.children:
                    if h.name == 'right':
                        for k in [[h.i + 1, h.j, 'right'], [h.i - 2, h.j, 'left']]:
                            if self.isClear(k[0], k[1]):
                                Node(name=k[2], i=k[0], j=k[1], parent=h)
                        if max(self.Ships) > 3:
                            for t in h.children:
                                if t.name == 'right':
                                    for k in [[t.i + 1, t.j, 'right'], [t.i - 3, t.j, 'left']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
                                elif t.name == 'left':
                                    for k in [[t.i + 3, t.j, 'right'], [t.i - 1, t.j, 'left']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
                    elif h.name == 'left':
                        for k in [[h.i + 2, h.j, 'right'], [h.i - 1, h.j, 'left']]:
                            if self.isClear(k[0], k[1]):
                                Node(name=k[2], i=k[0], j=k[1], parent=h)
                        if max(self.Ships) > 3:
                            for t in h.children:
                                if t.name == 'right':
                                    for k in [[t.i + 1, t.j, 'right'], [t.i - 3, t.j, 'left']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
                                elif t.name == 'left':
                                    for k in [[t.i + 3, t.j, 'right'], [t.i - 1, t.j, 'left']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
                    elif h.name == 'up':
                        for k in [[h.i, h.j-1, 'up'], [h.i, h.j+2, 'down']]:
                            if self.isClear(k[0], k[1]):
                                Node(name=k[2], i=k[0], j=k[1], parent=h)
                        if max(self.Ships) > 3:
                            for t in h.children:
                                if t.name == 'up':
                                    for k in [[t.i, t.j - 1, 'up'], [t.i, t.j + 3, 'down']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
                                elif t.name == 'down':
                                    for k in [[t.i, t.j - 3, 'up'], [t.i, t.j + 1, 'down']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
                    elif h.name == 'down':
                        for k in [[h.i, h.j - 2, 'up'], [h.i, h.j + 1, 'down']]:
                            if self.isClear(k[0], k[1]):
                                Node(name=k[2], i=k[0], j=k[1], parent=h)
                        if max(self.Ships) > 3:
                            for t in h.children:
                                if t.name == 'up':
                                    for k in [[t.i, t.j - 1, 'up'], [t.i, t.j + 3, 'down']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
                                elif t.name == 'down':
                                    for k in [[t.i, t.j - 3, 'up'], [t.i, t.j + 1, 'down']]:
                                        if self.isClear(k[0], k[1]):
                                            Node(name=k[2], i=k[0], j=k[1], parent=t)
        # При последующем попаданиии нод попадания становится рутом
        elif self.DamageTree != None:
            self.AI.parent = None
            self.DamageTree = self.AI




    def Missed(self):
        try:
            if (self.DamageTree != None):
                for i in findall(self.DamageTree, filter_=lambda node: node.i == self.AI.i and node.j == self.AI.j):
                    i.parent = None
                self.AI.parent = None
                self.AI = None
        except:
            d = "da"



    def Killed(self):
        # При убийстве затираем дерево дамага, удаляем корабль из списка и окружаем клетки вокруг корабля воздухом
        if len(self.DamageList) != max(self.Ships):
            self.DamageList.append(self.LastShot)
        self.DamageTree = None
        self.AI = None
        for i in self.DamageList:
            for j in self.Around(i):
                if self.isClear(j[0], j[1]):
                    self.Matrix[j[0]][j[1]].state = 1
        self.Ships.remove(len(self.DamageList))
        self.DamageList = []


    def isClear(self, i, j):
        # Проверка клетки на валидность (пустоту)
        try:
            if self.Matrix[i][j].state == 0 and (i in range(0, 10) and (j in range(0, 10))):
                return True
            else:
                return False
        except:
            return False


    def Decide(self):
        # Выбор клетки для стрельбы
        if self.DamageTree != None:
            # Отрезание невозможных нодов
            for i in self.DamageTree.children:
                if self.Ships.count(2) == 0 and self.Ships.count(3) == 0 and self.Ships.count(4) == 0 and i.height == 0:
                    i.parent = None
                if self.Ships.count(3) == 0 and self.Ships.count(4) == 0 and i.height == 1:
                    i.parent = None
                if self.Ships.count(4) == 0 and i.height == 2:
                    i.parent = None
            self.AI = self.DamageTree.children[random.randint(0, len(self.DamageTree.children) - 1)]
            return [self.AI.i, self.AI.j]
        else:
            # Достаем клетку из списка и работаем с ней
            #
            stayfrosty = False
            while not stayfrosty:
                pick = self.Tactic.pop()
                if not self.isClear(pick[0], pick[1]):
                   continue
                stayfrosty = True
                if self.Ships.count(2) == 0 and self.Ships.count(1) == 0 and self.Ships.count(3) == 0:
                    Tiles = self.ClearTile(self.ExtendAroundCrest(pick))
                    tilesx = list(filter(lambda x: x[1] == pick[1], Tiles))
                    tilesy = list(filter(lambda x: x[0] == pick[0], Tiles))
                    if len(tilesx) >= 3 or len(tilesy) >= 3:
                        stayfrosty = True
                    else:
                        stayfrosty = False
                if self.Ships.count(2) == 0 and self.Ships.count(1) == 0:
                    Tiles = self.ClearTile(self.AroundCrest(pick))
                    if len(Tiles) <= 1:
                        stayfrosty = False
                    elif len(Tiles) == 2:
                        if abs(Tiles[0][0] - Tiles[1][0]) == 1 and abs(Tiles[0][1] - Tiles[1][1]) == 1:
                            stayfrosty = False
                    else:
                        stayfrosty = True
                elif self.Ships.count(1) == 0:
                    if len(self.ClearTile(self.AroundCrest(pick))) == 0:
                        stayfrosty = False

            return [pick[0], pick[1]]


    def DefaultTactic(self):
        # Загрузка тактики стрельбы по диагонали
        l = []
        for i in range(0, 4):
            if (i < 4-2):
                l.append((i, 3-2-i))
                l.append((i+2, (6+3)-i))
            self.Tactic.append((i, 3-i))
            self.Tactic.append((6+i, 6+3-i))
        for i in range(0, 8):
            if (i < 6-2):
                l.append((i, 7-2-i))
                l.append((i+2, 7+2-i))
            self.Tactic.append((i, 7-i))
            self.Tactic.append((i, 7+2-i))
        for i in range(9, -1, -1):
            l.append((i, 9-i))
        random.shuffle(l)
        random.shuffle(self.Tactic)
        list = []
        for i in range(0, 10):
            for j in range(0, 10):
                if (i, j) in self.Tactic:
                    continue
                list.append((i, j))
        random.shuffle(list)
        self.Tactic += l + list
        self.Tactic.reverse()



battlefild = Battlefield()


if __name__ == "__main__":
    while battlefild.Ships != []:
        battlefild.VolleyD()
