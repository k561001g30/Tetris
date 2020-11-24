# Tetris
Tetris (Russian: Тетрис [ˈtɛtrʲɪs]) is a tile-matching video game created by Russian software engineer Alexey Pajitnov in 1984.

In Tetris, players complete lines by moving differently shaped pieces (tetrominoes), which descend onto the playing field. 

The completed lines disappear and grant the player points, and the player can proceed to fill the vacated spaces. 

The game ends when the playing field is filled. The longer the player can delay this inevitable outcome, the higher their score will be. 

[wiki]

In this project I will mapping Single player Tetris in C#

# In Program face


# In Gaming face 
There are 7 types of Block in Tetris Games in 4 ways to mapping

I,J,L,O,S,T,Z
# 4 ways Mapping

1. O

2. I

3. S,Z

4. J,L,S,Z

# Type 1:O to Array[4][4]

It will no change when Key in "UP".

So,in array it will take value as following

0,0,0,0

0,1,1,0

0,1,1,0

0,0,0,0

# Type 2:I to Array[4][4]

It will change the shape when Key in "UP".

So,in array it will take value as following

1,0,0,0     0,0,0,0

1,0,0,0 --> 0,0,0,0

1,0,0,0     0,0,0,0

1,0,0,0     1,1,1,1

# Type 3:S,Z to Array[4][4]

In array it will take value as following

0,0,0,0

0,1,1,0

0,1,1,0

0,0,0,0

# Type 1:O to Array[4][4]

It will no change when Key in "UP".

So,in array it will take value as following

0,0,0,0

0,1,1,0

0,1,1,0

0,0,0,0
