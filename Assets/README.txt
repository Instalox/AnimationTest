Fox Cub Games - Developer Animation Test

This project will:
* test your ability with the various ways of doing animations in Unity
* let you showcase your "making stuff look good" skills
* show that you can make intuitive inspector tools for changing prefab data

It consists of 3 parts:
1) a coin fountain
2) a particle comet that animates from the edge of the screen to where you click
3) a sprite that animates according to the provided mockup


Now, let me explain each part in more detail.

1) Coin Fountain - animation via code

For this, use the provided "coin" art asset.
The coin fountain should be toggleable on/off via a simple button somewhere on the screen.
When on, coins should spew from a point near the bottom of the screen, to the top of the screen.
They should have some random amount of horizontal movement.
The coins should move realistically (dragged down by gravity), just like they would in the real world.
They should not collide with each other, or anything else.
Do not use any Physics system for this.
I know that you could use a particle system to do this, but please don't. I'm looking to see if you can code this animation manually.

Look at "coin_fountain_mockup" to get an idea of what it should look like. Some of the yellow paths I have drawn might be unrealistic, so don't think you need to have complicated logic to make your coin paths match up exactly.

Tuning variables you should expose on a prefab:
* coin spew rate
* min & max horizontal movement
* min & max height

The end result doesn't have to be fancy, but it should look good and be something you'd be proud to put into your game.


2) Particle Comet - animation via tween or code

Clicking on the screen should launch a particle comet.
The launch point should be the edge of the screen (random side, random height from bottom to half the screen height).
The destination should be where you clicked.
When the comet arrives at it's destination, it simply fades out and triggers section 3, the Sprite Animation.
You can code this animation manually, but you could also use a tweening engine such as DOTween, LeanTween, etc.

Tuning variables you shoudl expose on a prefab:
* speed of the comet

The comet art could either be the same coin from section 1, or a particle system that you create. I don't care, but make it look good =)


3) Sprite Animation - animation via Unity Animator

When the particle comet hits from section 2, it starts the Sprite Animation.
For details on how this animation should look, please see the series of screenshots in the "3 sprite animation/mockup sequence" folder.
Use "big win explosion" for the sprite that you should animate.

The mockup has a lot of stuff going on - the animated sprite, the blue glow, the coin spew, the blur effect.
The only requirement I have is to animate the big win sprite via the Unity Animator.
Aside from that, you can make the animation as simple or as complicated as you want.
The mockup should give you some ideas about other things you could do to make it look good, or you can come up with your own brand new spin on it.
Again, the end result doesn't have to be fancy, but it should look good and be something you'd be proud to put into your game.