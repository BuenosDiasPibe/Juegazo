<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.2" name="worldTexture" tilewidth="8" tileheight="8" tilecount="128" columns="8">
 <image source="../Content/worldTexture.png" width="64" height="128"/>
 <tile id="1" type="JumpWallBlock"/>
 <tile id="5" type="SpeedUpBlock"/>
 <tile id="8" type="VerticalBoostBlock">
  <properties>
   <property name="Ammount" type="int" value="2"/>
  </properties>
 </tile>
 <tile id="10" type="SlowDownBlock"/>
 <tile id="11" type="CollisionBlock"/>
 <tile id="12" type="OneWayBlock"/>
 <tile id="13" type="DoubleJump"/>
 <tile id="15" type="CompleteLevelBlock">
  <properties>
   <property name="isEnabled" type="bool" value="true"/>
   <property name="nextLevel" type="int" value="0"/>
  </properties>
 </tile>
 <tile id="16" type="MovementBlock">
  <animation>
   <frame tileid="11" duration="150"/>
   <frame tileid="18" duration="150"/>
   <frame tileid="16" duration="150"/>
   <frame tileid="24" duration="150"/>
  </animation>
 </tile>
 <tile id="17" type="CheckPointBlock"/>
 <tile id="18" type="DamageBlock">
  <properties>
   <property name="canDamage" type="bool" value="true"/>
   <property name="damageAmount" type="int" value="1"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="2" name="coso" type="DamageBlock" x="1.90923" y="0.886427" width="2.95419" height="4.04517"/>
  </objectgroup>
  <animation>
   <frame tileid="18" duration="150"/>
   <frame tileid="2" duration="150"/>
   <frame tileid="2" duration="150"/>
   <frame tileid="3" duration="150"/>
   <frame tileid="4" duration="150"/>
  </animation>
 </tile>
 <tile id="22" type="DamageBlock"/>
 <tile id="27" type="DoorBlock"/>
 <tile id="28" type="Key"/>
</tileset>
