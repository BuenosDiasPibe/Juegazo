<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.2" name="worldSpace2" tilewidth="16" tileheight="16" tilecount="128" columns="8">
 <image source="worldSprites.png" width="128" height="256"/>
 <tile id="1" type="JumpWallBlock"/>
 <tile id="5" type="speedUp"/>
 <tile id="8" type="VerticalBoostBlock"/>
 <tile id="10" type="SlowDownBlock"/>
 <tile id="11" type="Collision Block"/>
 <tile id="12" type="OneWayBlock"/>
 <tile id="15" type="CompleteLevelBlock"/>
 <tile id="16" type="MovementBlock"/>
 <tile id="17" type="CheckPointBlock"/>
 <tile id="18" type="DamageBlock">
  <animation>
   <frame tileid="18" duration="150"/>
   <frame tileid="19" duration="150"/>
   <frame tileid="20" duration="150"/>
  </animation>
 </tile>
 <tile id="19" type="DamageBlock"/>
 <tile id="20" type="DamageBlock"/>
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
 </tile>
 <tile id="35" type="NPC">
  <properties>
   <property name="name" value="Jose"/>
  </properties>
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
 </tile>
 <tile id="42" type="NPC">
  <properties>
   <property name="name" value="Grandma"/>
  </properties>
 </tile>
</tileset>
