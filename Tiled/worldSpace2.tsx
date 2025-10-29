<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.2" name="worldSpace2" tilewidth="16" tileheight="16" tilecount="128" columns="8">
 <image source="worldSprites.png" width="128" height="256"/>
 <tile id="1" type="JumpWallBlock"/>
 <tile id="5" type="SpeedUpBlock"/>
 <tile id="8" type="VerticalBoostBlock">
  <animation>
   <frame tileid="8" duration="150"/>
   <frame tileid="53" duration="150"/>
   <frame tileid="18" duration="150"/>
   <frame tileid="20" duration="150"/>
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
 <tile id="11" type="CollisionBlock"/>
 <tile id="12" type="OneWayBlock"/>
 <tile id="13" type="DoubleJump"/>
 <tile id="14" type="BouncerMode"/>
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
  <objectgroup draworder="index" id="2">
   <object id="1" x="4.72727" y="3.27273" width="7.36364" height="10.5455"/>
  </objectgroup>
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
  <objectgroup draworder="index" id="2">
   <object id="1" x="3.18182" y="4.27273" width="8.27273" height="9.27273"/>
  </objectgroup>
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
  <objectgroup draworder="index" id="2">
   <object id="1" x="7.09091" y="3.72727" width="4.72727" height="10.4545"/>
  </objectgroup>
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
 <tile id="23" type="EntityChanger">
  <properties>
   <property name="idk" type="int" value="1"/>
  </properties>
 </tile>
 <tile id="27" type="DoorBlock"/>
 <tile id="28" type="Key"/>
 <tile id="29" type="JumpingOrb">
  <properties>
   <property name="JumpAmmount" type="int" value="10"/>
   <property name="isUsable" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="30" type="GravityChangerOrbBlock">
  <properties>
   <property name="changeVertical" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="31" type="GravityChangerOrbBlock">
  <properties>
   <property name="changeVertical" type="bool" value="true"/>
  </properties>
 </tile>
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
 <tile id="36" type="GravityChangerOrbBlock">
  <properties>
   <property name="changeHorizontal" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="37" type="GravityChangerOrbBlock">
  <properties>
   <property name="changeHorizontal" type="bool" value="true"/>
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
 <tile id="43" type="GravityChangerPadBlock"/>
 <tile id="44" type="GravityChangerPadBlock">
  <properties>
   <property name="snapTo" type="int" propertytype="FACES" value="2"/>
  </properties>
 </tile>
 <tile id="45" type="GravityChangerPadBlock">
  <properties>
   <property name="snapTo" type="int" propertytype="FACES" value="1"/>
  </properties>
 </tile>
 <tile id="46" type="GravityChangerPadBlock">
  <properties>
   <property name="snapTo" type="int" propertytype="FACES" value="3"/>
  </properties>
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
 <tile id="50" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="50" duration="150"/>
   <frame tileid="51" duration="150"/>
   <frame tileid="52" duration="150"/>
   <frame tileid="53" duration="150"/>
   <frame tileid="54" duration="150"/>
   <frame tileid="55" duration="150"/>
  </animation>
 </tile>
 <tile id="51" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="50" duration="150"/>
   <frame tileid="51" duration="150"/>
   <frame tileid="52" duration="150"/>
   <frame tileid="53" duration="150"/>
   <frame tileid="54" duration="150"/>
   <frame tileid="55" duration="150"/>
  </animation>
 </tile>
 <tile id="52" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="50" duration="150"/>
   <frame tileid="51" duration="150"/>
   <frame tileid="52" duration="150"/>
   <frame tileid="53" duration="150"/>
   <frame tileid="54" duration="150"/>
   <frame tileid="55" duration="150"/>
  </animation>
 </tile>
 <tile id="53" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="50" duration="150"/>
   <frame tileid="51" duration="150"/>
   <frame tileid="52" duration="150"/>
   <frame tileid="53" duration="150"/>
   <frame tileid="54" duration="150"/>
   <frame tileid="55" duration="150"/>
  </animation>
 </tile>
 <tile id="54" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="50" duration="150"/>
   <frame tileid="51" duration="150"/>
   <frame tileid="52" duration="150"/>
   <frame tileid="53" duration="150"/>
   <frame tileid="54" duration="150"/>
   <frame tileid="55" duration="150"/>
  </animation>
 </tile>
 <tile id="55" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="50" duration="150"/>
   <frame tileid="51" duration="150"/>
   <frame tileid="52" duration="150"/>
   <frame tileid="53" duration="150"/>
   <frame tileid="54" duration="150"/>
   <frame tileid="55" duration="150"/>
  </animation>
 </tile>
 <tile id="56" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="58" duration="150"/>
   <frame tileid="56" duration="150"/>
   <frame tileid="57" duration="150"/>
   <frame tileid="59" duration="150"/>
   <frame tileid="60" duration="150"/>
   <frame tileid="61" duration="150"/>
  </animation>
 </tile>
 <tile id="57" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="58" duration="150"/>
   <frame tileid="56" duration="150"/>
   <frame tileid="57" duration="150"/>
   <frame tileid="59" duration="150"/>
   <frame tileid="60" duration="150"/>
   <frame tileid="61" duration="150"/>
  </animation>
 </tile>
 <tile id="58" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="56" duration="150"/>
   <frame tileid="57" duration="150"/>
   <frame tileid="58" duration="150"/>
   <frame tileid="59" duration="150"/>
   <frame tileid="60" duration="150"/>
   <frame tileid="61" duration="150"/>
  </animation>
 </tile>
 <tile id="59" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="56" duration="150"/>
   <frame tileid="57" duration="150"/>
   <frame tileid="58" duration="150"/>
   <frame tileid="59" duration="150"/>
   <frame tileid="60" duration="150"/>
   <frame tileid="61" duration="150"/>
  </animation>
 </tile>
 <tile id="60" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
  <animation>
   <frame tileid="56" duration="150"/>
   <frame tileid="57" duration="150"/>
   <frame tileid="58" duration="150"/>
   <frame tileid="59" duration="150"/>
   <frame tileid="60" duration="150"/>
   <frame tileid="61" duration="150"/>
  </animation>
 </tile>
 <tile id="61" type="Portal">
  <properties>
   <property name="delayTimeSeconds" type="float" value="3"/>
  </properties>
 </tile>
 <tile id="64" type="WaterBlock">
  <animation>
   <frame tileid="66" duration="150"/>
   <frame tileid="65" duration="150"/>
   <frame tileid="66" duration="150"/>
  </animation>
 </tile>
 <tile id="65" type="WaterBlock">
  <animation>
   <frame tileid="66" duration="150"/>
   <frame tileid="65" duration="150"/>
   <frame tileid="64" duration="150"/>
  </animation>
 </tile>
 <tile id="66" type="WaterBlock">
  <animation>
   <frame tileid="64" duration="150"/>
   <frame tileid="65" duration="150"/>
   <frame tileid="66" duration="150"/>
  </animation>
 </tile>
 <tile id="72" type="MovingDamageBlock">
  <animation>
   <frame tileid="72" duration="250"/>
   <frame tileid="73" duration="250"/>
   <frame tileid="74" duration="250"/>
   <frame tileid="75" duration="250"/>
  </animation>
 </tile>
 <tile id="73" type="MovingDamageBlock">
  <animation>
   <frame tileid="72" duration="250"/>
   <frame tileid="73" duration="250"/>
   <frame tileid="74" duration="250"/>
   <frame tileid="75" duration="250"/>
  </animation>
 </tile>
 <tile id="74" type="MovingDamageBlock">
  <animation>
   <frame tileid="72" duration="250"/>
   <frame tileid="73" duration="250"/>
   <frame tileid="74" duration="250"/>
   <frame tileid="75" duration="250"/>
  </animation>
 </tile>
 <tile id="75" type="MovingDamageBlock">
  <animation>
   <frame tileid="72" duration="200"/>
   <frame tileid="73" duration="200"/>
   <frame tileid="74" duration="200"/>
   <frame tileid="75" duration="200"/>
  </animation>
 </tile>
 <tile id="80" type="JumpingOrb">
  <properties>
   <property name="JumpAmmount" type="int" value="20"/>
   <property name="isUsable" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="81" type="JumpingOrb">
  <properties>
   <property name="JumpAmmount" type="int" value="5"/>
   <property name="isUsable" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="88" type="GravityChangerMode">
  <properties>
   <property name="changeVertical" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="89" type="GravityChangerMode">
  <properties>
   <property name="changeHorizontal" type="bool" value="true"/>
   <property name="changeVertical" type="bool" value="false"/>
  </properties>
 </tile>
 <tile id="90" type="GravityChangerMode">
  <properties>
   <property name="changeHorizontal" type="bool" value="true"/>
   <property name="changeVertical" type="bool" value="true"/>
  </properties>
 </tile>
</tileset>
