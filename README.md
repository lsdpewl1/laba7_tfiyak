# Лабораторная работа №7
# Вариант 14
## Грамматика
G[STMT]:
1. STMT :== if “(” EXPR “)” STMT | if “(” EXPR “)” STMT else
STMT | ε
2. EXPR :== VARIABLE OPERATOR VALUE</br>
VALUE – числовая константа Ц{Ц}, VARIABLE – переменная
Б{Б|Ц}, Б – {a, b, c, ...z, A, B, …, Z}, Ц – {0, 1, …, 9}, OPERATOR – ”==” |
”<” | ”<=” | ”>” | ”>=” | ”!=”
## Язык
L(G[STMT]) = {((if (EXPR))^n (else)^m)^k | n > 0 m>=0 k>0}
## Классификация грамматики
Контексто-свободная
## Тестовые примеры
<img src = "1.png">
<img src = "2.png">
<img src = "3.png">
<img src = "4.png">