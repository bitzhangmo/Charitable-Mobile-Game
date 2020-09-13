### 拆字表格生成脚本

#### 目标：

- 输入：一个txt文件，其中按照某种格式填写目标汉字。
- 输出：一个txt文件，其中按照某种格式填写目标汉字所能拆成的汉字偏旁部首，以及这些偏旁部首所能组成的新的字，这个文件中的每一个元素所构成的集合，应符合封闭性，把字与字相拼看作一种运算，元素间的这种运算结果应包含在这个集合中。

#### 思路：

1. 需要有一个字典1，其中包含了拆字的规则，即一个汉字可以被拆解成哪些偏旁部首。 done

   ```lua
   -- 字典形如：
   仫	人 么	亻 么
   仭	人 刄	亻 刄
   仮	人 反	亻 反
   仯	人 少	亻 少
   仰	人 卬	亻 卬
   仱	人 今	亻 今
   仲	人 中	亻 中
   仳	人 比	亻 比
   仴	人 月	亻 月
   仵	人 午	亻 午
   件	人 牛	亻 牛
   仸	人 夭	亻 夭
   仹	人 丰	亻 丰
   ```

2. 需要对字典进行逆转操作，即生成一个通过某个偏旁部首共能组成哪些字的字典2，作为中间文件1 done

   ```lua
   -- 中间文件1形如：
   ├─ 寺
   │  ├─ 1
   │  │  ├─ 1: 2		--	拼成该字需要经过的步数n
   │  │  └─ 2: "侍"
   │  ├─ 2
   │  │  ├─ 1: 2
   │  │  └─ 2: "侍"
   │  ├─ 3
   │  │  ├─ 1: 2
   │  │  └─ 2: "峙"
   │  ├─ 4
   │  │  ├─ 1: 2
   │  │  └─ 2: "庤"
   │  ├─ 5
   │  │  ├─ 1: 2
   │  │  └─ 2: "待"
   │  ├─ 6
   │  │  ├─ 1: 2
   │  │  └─ 2: "恃"
   │  ├─ 7
   │  │  ├─ 1: 2
   │  │  └─ 2: "恃"
   │  ├─ 8
   │  │  ├─ 1: 2
   │  │  └─ 2: "持"
   │  ├─ 9
   │  │  ├─ 1: 2
   │  │  └─ 2: "持"
   │  ├─ 10
   │  │  ├─ 1: 2
   │  │  └─ 2: "持"
   │  ├─ 11
   │  │  ├─ 1: 2
   │  │  └─ 2: "歭"
   │  ├─ 12
   │  │  ├─ 1: 2
   │  │  └─ 2: "洔"
   │  ├─ 13
   │  │  ├─ 1: 2
   │  │  └─ 2: "洔"
   │  ├─ 14
   │  │  ├─ 1: 2
   │  │  └─ 2: "特"
   │  ├─ 15
   │  │  ├─ 1: 2
   │  │  └─ 2: "特"
   │  ├─ 16
   │  │  ├─ 1: 2
   │  │  └─ 2: "畤"
   │  ├─ 17
   │  │  ├─ 1: 2
   │  │  └─ 2: "痔"
   │  ├─ 18
   │  │  ├─ 1: 2
   │  │  └─ 2: "痔"
   │  ├─ 19
   │  │  ├─ 1: 2
   │  │  └─ 2: "秲"
   │  ├─ 20
   │  │  ├─ 1: 2
   │  │  └─ 2: "等"
   │  ├─ 21
   │  │  ├─ 1: 2
   │  │  └─ 2: "诗"
   │  ├─ 22
   │  │  ├─ 1: 2
   │  │  └─ 2: "诗"
   │  ├─ 23
   │  │  ├─ 1: 2
   │  │  └─ 2: "跱"
   │  ├─ 24
   │  │  ├─ 1: 2
   │  │  └─ 2: "邿"
   │  ├─ 25
   │  │  ├─ 1: 2
   │  │  └─ 2: "邿"
   │  └─ 26
   │     ├─ 1: 2
   │     └─ 2: "鼭"
   ```

3. 需要在字典1中寻找到输入文件中包含的汉字，作为中间文件2，此文件限定了输入文件中的文字可以被拆成哪些偏旁部首，这个文件应包含偏旁部首和目标汉字本身。 done

4. 需要通过中间文件2在中间文件1中进行查找并输出，作为中间文件3，此文件限定了中间文件2中的字体组件共能组成哪些字，其中key为字体组件，value为由该组件能组成哪些字的集合。done

5. 需要对中间文件3中的value部分进行统计，仅出现一次的value说明仅有一个组件可以组成该字，不符合拼字规则，可以进行剔除，最后剩下的汉字输出到中间文件4中，中间文件4中出现的汉字即为在游戏中能出现的所有汉字，该文件也会作为Text Mesh生成的源文件。 done

6. 最后需要遍历中间文件3的value部分，不在中间文件4中的value剔除，生成一个key为字体组件，value为组成字的集合的文件，作为最终的输出文件。

#### 相关参考：