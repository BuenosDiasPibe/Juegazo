<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.2" name="worldSpace2" tilewidth="16" tileheight="16" tilecount="128" columns="8">
 <image source="worldSprites.png" width="128" height="256"/>
 <tile id="1" type="JumpWallBlock"/>
 <tile id="5" type="speedUp"/>
 <tile id="8" type="VerticalBoostBlock">
  <animation>
   <frame tileid="8" duration="150"/>
   <frame tileid="14" duration="150"/>
  </animation>
 </tile>
 <tile id="9" type="DamageBlock">
  <properties>
   <property name="canDamage" type="bool" value="true"/>
   <property name="damageAmount" type="int" value="1"/>
  </properties>
  <animation>
   <frame tileid="17" duration="150"/>
   <frame tileid="17" duration="150"/>
   <frame tileid="25" duration="150"/>
   <frame tileid="32" duration="150"/>
   <frame tileid="42" duration="150"/>
  </animation>
 </tile>
 <tile id="10" type="SlowDownBlock"/>
 <tile id="11" type="Collision Block"/>
 <tile id="12" type="OneWayBlock"/>
 <tile id="13" type="DoubleJump"/>
 <tile id="15" type="CompleteLevelBlock"/>
 <tile id="16" type="MovementBlock">
  <animation>
   <frame tileid="16" duration="150"/>
   <frame tileid="26" duration="150"/>
   <frame tileid="18" duration="150"/>
  </animation>
 </tile>
 <tile id="17" type="CheckPointBlock"/>
 <tile id="18" type="DamageBlock">
  <properties>
   <property name="canDamage" type="bool" value="true"/>
   <property name="damageAmount" type="int" value="1"/>
  </properties>
  <animation>
   <frame tileid="18" duration="150"/>
   <frame tileid="19" duration="150"/>
   <frame tileid="20" duration="150"/>
  </animation>
 </tile>
 <tile id="19" type="DamageBlock">
  <properties>
   <property name="canDamage" type="bool" value="true"/>
   <property name="damageAmount" type="int" value="1"/>
  </properties>
  <animation>
   <frame tileid="20" duration="150"/>
   <frame tileid="18" duration="150"/>
   <frame tileid="19" duration="150"/>
  </animation>
 </tile>
 <tile id="20" type="DamageBlock">
  <properties>
   <property name="canDamage" type="bool" value="true"/>
   <property name="damageAmount" type="int" value="1"/>
  </properties>
  <animation>
   <frame tileid="20" duration="150"/>
   <frame tileid="18" duration="150"/>
   <frame tileid="19" duration="150"/>
  </animation>
 </tile>
 <tile id="21" type="Gunner">
  <animation>
   <frame tileid="21" duration="150"/>
   <frame tileid="22" duration="150"/>
   <frame tileid="20" duration="150"/>
  </animation>
 </tile>
 <tile id="22" type="Gunner">
  <animation>
   <frame tileid="22" duration="150"/>
   <frame tileid="21" duration="150"/>
  </animation>
 </tile>
 <tile id="27" type="DoorBlock"/>
 <tile id="28" type="Key"/>
 <tile id="32" type="NPC">
  <properties>
   <property name="name" value="Jose"/>
  </properties>
  <animation>
   <frame tileid="32" duration="150"/>
   <frame tileid="33" duration="150"/>
   <frame tileid="34" duration="150"/>
   <frame tileid="35" duration="150"/>
  </animation>
 </tile>
 <tile id="33" type="NPC">
  <properties>
   <property name="name" value="Jose"/>
  </properties>
  <animation>
   <frame tileid="34" duration="150"/>
   <frame tileid="33" duration="150"/>
   <frame tileid="32" duration="150"/>
   <frame tileid="35" duration="150"/>
  </animation>
 </tile>
 <tile id="34" type="NPC">
  <properties>
   <property name="name" value="Jose"/>
  </properties>
  <animation>
   <frame tileid="32" duration="150"/>
   <frame tileid="33" duration="150"/>
  </animation>
 </tile>
 <tile id="35" type="NPC">
  <properties>
   <property name="name" value="Jose"/>
  </properties>
  <animation>
   <frame tileid="35" duration="150"/>
   <frame tileid="34" duration="150"/>
  </animation>
 </tile>
 <tile id="40" type="NPC">
  <properties>
   <property name="name" value="Grandma"/>
  </properties>
  <animation>
   <frame tileid="40" duration="100"/>
   <frame tileid="41" duration="100"/>
   <frame tileid="42" duration="100"/>
  </animation>
 </tile>
 <tile id="41" type="NPC">
  <properties>
   <property name="name" value="Grandma"/>
  </properties>
  <animation>
   <frame tileid="40" duration="150"/>
   <frame tileid="41" duration="150"/>
   <frame tileid="42" duration="150"/>
  </animation>
 </tile>
 <tile id="42" type="NPC">
  <properties>
   <property name="name" value="Grandma"/>
  </properties>
  <animation>
   <frame tileid="42" duration="150"/>
   <frame tileid="40" duration="150"/>
   <frame tileid="41" duration="150"/>
  </animation>
 </tile>
 <tile id="48" type="MoveOneDirection">
  <properties>
   <property name="canMove" type="bool" value="false"/>
   <property name="velocity" type="int" value="0"/>
  </properties>
  <animation>
   <frame tileid="48" duration="150"/>
   <frame tileid="49" duration="150"/>
  </animation>
 </tile>
 <tile id="49" type="MoveOneDirection">
  <properties>
   <property name="canMove" type="bool" value="false"/>
   <property name="velocity" type="int" value="0"/>
  </properties>
 </tile>
</tileset>
