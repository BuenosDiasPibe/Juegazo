<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.11.2" name="worldTexture" tilewidth="8" tileheight="8" tilecount="128" columns="8">
 <image source="../Content/worldTexture.png" width="64" height="128"/>
 <tile id="1" type="JumpWallBlock"/>
 <tile id="5" type="speedUp"/>
 <tile id="8" type="VerticalBoostBlock"/>
 <tile id="10" type="SlowDownBlock"/>
 <tile id="11" type="Collision Block"/>
 <tile id="12" type="OneWayBlock"/>
 <tile id="15" type="CompleteLevelBlock">
  <properties>
   <property name="isEnabled" type="bool" value="false"/>
   <property name="nextLevel" type="int" value="1"/>
  </properties>
 </tile>
 <tile id="16" type="MovementBlock"/>
 <tile id="17" type="CheckPointBlock"/>
 <tile id="18" type="DamageBlock"/>
 <tile id="22" type="DamageBlock"/>
 <tile id="27" type="DoorBlock"/>
 <tile id="28" type="Key"/>
</tileset>
