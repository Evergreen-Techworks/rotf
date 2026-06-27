package com.company.assembleegameclient.util
{
   import com.company.assembleegameclient.engine3d.Model3D;
   import com.company.assembleegameclient.map.GroundLibrary;
   import com.company.assembleegameclient.map.RegionLibrary;
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import com.company.assembleegameclient.objects.particles.ParticleLibrary;
   import com.company.assembleegameclient.parameters.Parameters;
   import com.company.assembleegameclient.sound.Music;
   import com.company.assembleegameclient.sound.SFX;
   import com.company.assembleegameclient.sound.SoundEffectLibrary;
   import com.company.util.AssetLibrary;
   import flash.display.BitmapData;
   import flash.utils.ByteArray;
   import kabam.rotmg.assets.EmbeddedAssets;
   import kabam.rotmg.assets.EmbeddedData;
   
   public class AssetLoader
   {
       
      
      public function AssetLoader()
      {
         super();
      }
      
      public function load() : void
      {
         this.addImages();
         this.addAnimatedCharacters();
         this.addSoundEffects();
         this.parse3DModels();
         this.parseParticleEffects();
         this.parseGroundFiles();
         this.parseObjectFiles();
         this.parseRegionFiles();
         Parameters.load();
         Music.load();
         SFX.load();
      }
      
      private function addImages() : void
      {
         AssetLibrary.addImageSet("lofiChar8x8",new EmbeddedAssets.lofiCharEmbed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiChar16x8",new EmbeddedAssets.lofiCharEmbed_().bitmapData,16,8);
         AssetLibrary.addImageSet("lofiChar16x16",new EmbeddedAssets.lofiCharEmbed_().bitmapData,16,16);
         AssetLibrary.addImageSet("lofiChar28x8",new EmbeddedAssets.lofiChar2Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiChar216x8",new EmbeddedAssets.lofiChar2Embed_().bitmapData,16,8);
         AssetLibrary.addImageSet("lofiChar216x16",new EmbeddedAssets.lofiChar2Embed_().bitmapData,16,16);
         AssetLibrary.addImageSet("lofiCharBig",new EmbeddedAssets.lofiCharBigEmbed_().bitmapData,16,16);
         AssetLibrary.addImageSet("lofiEnvironment",new EmbeddedAssets.lofiEnvironmentEmbed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiEnvironment2",new EmbeddedAssets.lofiEnvironment2Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiEnvironment3",new EmbeddedAssets.lofiEnvironment3Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiInterface",new EmbeddedAssets.lofiInterfaceEmbed_().bitmapData,8,8);
         AssetLibrary.addImageSet("redLootBag",new EmbeddedAssets.redLootBagEmbed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiInterfaceBig",new EmbeddedAssets.lofiInterfaceBigEmbed_().bitmapData,16,16);
         AssetLibrary.addImageSet("lofiInterface2",new EmbeddedAssets.lofiInterface2Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiObj",new EmbeddedAssets.lofiObjEmbed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiObj2",new EmbeddedAssets.lofiObj2Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiObj3",new EmbeddedAssets.lofiObj3Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiObj4",new EmbeddedAssets.lofiObj4Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiObj5",new EmbeddedAssets.lofiObj5Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiObj6",new EmbeddedAssets.lofiObj6Embed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiObjBig",new EmbeddedAssets.lofiObjBigEmbed_().bitmapData,16,16);
         AssetLibrary.addImageSet("lofiObj40x40",new EmbeddedAssets.lofiObj40x40Embed_().bitmapData,40,40);
         AssetLibrary.addImageSet("lofiProjs",new EmbeddedAssets.lofiProjsEmbed_().bitmapData,8,8);
         AssetLibrary.addImageSet("lofiProjsBig",new EmbeddedAssets.lofiProjsBigEmbed_().bitmapData,16,16);
         AssetLibrary.addImageSet("lofiParts",new EmbeddedAssets.lofiPartsEmbed_().bitmapData,8,8);
         AssetLibrary.addImageSet("stars",new EmbeddedAssets.starsEmbed_().bitmapData,5,5);
         AssetLibrary.addImageSet("textile4x4",new EmbeddedAssets.textile4x4Embed_().bitmapData,4,4);
         AssetLibrary.addImageSet("textile5x5",new EmbeddedAssets.textile5x5Embed_().bitmapData,5,5);
         AssetLibrary.addImageSet("textile9x9",new EmbeddedAssets.textile9x9Embed_().bitmapData,9,9);
         AssetLibrary.addImageSet("textile10x10",new EmbeddedAssets.textile10x10Embed_().bitmapData,10,10);
         AssetLibrary.addImageSet("inner_mask",new EmbeddedAssets.innerMaskEmbed_().bitmapData,4,4);
         AssetLibrary.addImageSet("sides_mask",new EmbeddedAssets.sidesMaskEmbed_().bitmapData,4,4);
         AssetLibrary.addImageSet("outer_mask",new EmbeddedAssets.outerMaskEmbed_().bitmapData,4,4);
         AssetLibrary.addImageSet("innerP1_mask",new EmbeddedAssets.innerP1MaskEmbed_().bitmapData,4,4);
         AssetLibrary.addImageSet("innerP2_mask",new EmbeddedAssets.innerP2MaskEmbed_().bitmapData,4,4);
         AssetLibrary.addImageSet("invisible",new BitmapData(8,8,true,0),8,8);
         // --- ROTF imported sprite sheets (16x16 grid; 256x208 => 16 cols x 13 rows) ---
         AssetLibrary.addImageSet("rotfBoss",new EmbeddedAssets.rotfBossEmbed_().bitmapData,16,16);
         // --- DECA sheets the ROTF data needs (from drips.pw); GUARDED so one bad sheet
         //     can never halt startup / black-screen the menu. petsDivine dropped (huge, 1 use). ---
         try { AssetLibrary.addImageSet("oryxHordeObjects8x8",new EmbeddedAssets.oryxHordeObjects8x8Embed_().bitmapData,8,8); } catch(e:Error) {}
         try { AssetLibrary.addImageSet("oryxHordeObjects16x16",new EmbeddedAssets.oryxHordeObjects16x16Embed_().bitmapData,16,16); } catch(e:Error) {}
         try { AssetLibrary.addImageSet("buffedBunnyObjects8x8",new EmbeddedAssets.buffedBunnyObjects8x8Embed_().bitmapData,8,8); } catch(e:Error) {}
         try { AssetLibrary.addImageSet("buffedBunnyObjects16x16",new EmbeddedAssets.buffedBunnyObjects16x16Embed_().bitmapData,16,16); } catch(e:Error) {}
         try { AssetLibrary.addImageSet("mountainTempleObjects8x8",new EmbeddedAssets.mountainTempleObjects8x8Embed_().bitmapData,8,8); } catch(e:Error) {}
         try { AssetLibrary.addImageSet("d3LofiObjEmbed",new EmbeddedAssets.d3LofiObjEmbed_().bitmapData,8,8); } catch(e:Error) {}
         try { AssetLibrary.addImageSet("d3LofiObjEmbed16",new EmbeddedAssets.d3LofiObjEmbed_().bitmapData,16,16); } catch(e:Error) {}
         try { AssetLibrary.addImageSet("d2LofiObjEmbed",new EmbeddedAssets.d2LofiObjEmbed_().bitmapData,8,8); } catch(e:Error) {}
      }
      
      private function addAnimatedCharacters() : void
      {
         AnimatedChars.add("chars8x8rBeach",new EmbeddedAssets.chars8x8rBeachEmbed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8dBeach",new EmbeddedAssets.chars8x8dBeachEmbed_().bitmapData,null,8,8,56,8,AnimatedChar.DOWN);
         AnimatedChars.add("chars8x8rLow1",new EmbeddedAssets.chars8x8rLow1Embed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8rLow2",new EmbeddedAssets.chars8x8rLow2Embed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8rMid",new EmbeddedAssets.chars8x8rMidEmbed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8rMid2",new EmbeddedAssets.chars8x8rMid2Embed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8rHigh",new EmbeddedAssets.chars8x8rHighEmbed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8rHero1",new EmbeddedAssets.chars8x8rHero1Embed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8rHero2",new EmbeddedAssets.chars8x8rHero2Embed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8dHero1",new EmbeddedAssets.chars8x8dHero1Embed_().bitmapData,null,8,8,56,8,AnimatedChar.DOWN);
         AnimatedChars.add("chars16x16dMountains1",new EmbeddedAssets.chars16x16dMountains1Embed_().bitmapData,null,16,16,112,16,AnimatedChar.DOWN);
         AnimatedChars.add("chars16x16dMountains2",new EmbeddedAssets.chars16x16dMountains2Embed_().bitmapData,null,16,16,112,16,AnimatedChar.DOWN);
         AnimatedChars.add("chars8x8dEncounters",new EmbeddedAssets.chars8x8dEncountersEmbed_().bitmapData,null,8,8,56,8,AnimatedChar.DOWN);
         AnimatedChars.add("chars8x8rEncounters",new EmbeddedAssets.chars8x8rEncountersEmbed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT);
         AnimatedChars.add("chars16x8dEncounters",new EmbeddedAssets.chars16x8dEncountersEmbed_().bitmapData,null,16,8,112,8,AnimatedChar.DOWN);
         AnimatedChars.add("chars16x8rEncounters",new EmbeddedAssets.chars16x8rEncountersEmbed_().bitmapData,null,16,8,112,8,AnimatedChar.DOWN);
         AnimatedChars.add("chars16x16dEncounters",new EmbeddedAssets.chars16x16dEncountersEmbed_().bitmapData,null,16,16,112,16,AnimatedChar.DOWN);
         AnimatedChars.add("chars16x16dEncounters2",new EmbeddedAssets.chars16x16dEncounters2Embed_().bitmapData,null,16,16,112,16,AnimatedChar.DOWN);
         AnimatedChars.add("chars16x16rEncounters",new EmbeddedAssets.chars16x16rEncountersEmbed_().bitmapData,null,16,16,112,16,AnimatedChar.RIGHT);
         AnimatedChars.add("players",new EmbeddedAssets.playersEmbed_().bitmapData,new EmbeddedAssets.playersMaskEmbed_().bitmapData,8,8,56,24,AnimatedChar.RIGHT);
         AnimatedChars.add("playerskins",new EmbeddedAssets.playersSkinsEmbed_().bitmapData,new EmbeddedAssets.playersSkinsMaskEmbed_().bitmapData,8,8,56,24,AnimatedChar.RIGHT);
         AnimatedChars.add("chars8x8rPets1",new EmbeddedAssets.chars8x8rPets1Embed_().bitmapData,new EmbeddedAssets.chars8x8rPets1MaskEmbed_().bitmapData,8,8,56,8,AnimatedChar.RIGHT);
         // --- ROTF: DECA animated char sheets (from drips.pw); added LAST + GUARDED so a
         //     bad sheet can never block the vanilla/player char registrations above (black menu). ---
         try { AnimatedChars.add("oryxHordeChars8x8",new EmbeddedAssets.oryxHordeChars8x8Embed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT); } catch(e:Error) {}
         try { AnimatedChars.add("oryxHordeChars16x16",new EmbeddedAssets.oryxHordeChars16x16Embed_().bitmapData,null,16,16,112,16,AnimatedChar.RIGHT); } catch(e:Error) {}
         try { AnimatedChars.add("buffedBunnyChars16x16",new EmbeddedAssets.buffedBunnyChars16x16Embed_().bitmapData,null,16,16,112,16,AnimatedChar.RIGHT); } catch(e:Error) {}
         try { AnimatedChars.add("d3Chars8x8rEmbed",new EmbeddedAssets.d3Chars8x8rEmbed_().bitmapData,null,8,8,56,8,AnimatedChar.RIGHT); } catch(e:Error) {}
         try { AnimatedChars.add("mountainTempleChars16x16",new EmbeddedAssets.mountainTempleChars16x16Embed_().bitmapData,null,16,16,112,16,AnimatedChar.RIGHT); } catch(e:Error) {}
      }
      
      private function addSoundEffects() : void
      {
         SoundEffectLibrary.load("button_click");
         SoundEffectLibrary.load("death_screen");
         SoundEffectLibrary.load("enter_realm");
         SoundEffectLibrary.load("error");
         SoundEffectLibrary.load("inventory_move_item");
         SoundEffectLibrary.load("level_up");
         SoundEffectLibrary.load("loot_appears");
         SoundEffectLibrary.load("no_mana");
         SoundEffectLibrary.load("use_key");
         SoundEffectLibrary.load("use_potion");
      }
      
      private function parse3DModels() : void
      {
         var name:* = null;
         var ba:ByteArray = null;
         var model:String = null;
         for(name in EmbeddedAssets.models_)
         {
            ba = EmbeddedAssets.models_[name];
            model = ba.readUTFBytes(ba.length);
            Model3D.parse3DOBJ(name, ba);
            Model3D.parseFromOBJ(name, model);
         }
      }
      
      private function parseParticleEffects() : void
      {
         var xml:XML = XML(new EmbeddedAssets.particlesEmbed());
         ParticleLibrary.parseFromXML(xml);
      }
      
      private function parseGroundFiles() : void
      {
         var groundObj:* = undefined;
         for each(groundObj in EmbeddedData.groundFiles)
         {
            GroundLibrary.parseFromXML(XML(groundObj));
         }
      }
      
      private function parseObjectFiles() : void
      {
         var objectObj:* = undefined;
         for each(objectObj in EmbeddedData.objectFiles)
         {
            try { ObjectLibrary.parseFromXML(XML(objectObj)); } catch(e:Error) {}
         }
      }
      
      private function parseRegionFiles() : void
      {
         var regionXML:* = undefined;
         for each(regionXML in EmbeddedData.regionFiles)
         {
            RegionLibrary.parseFromXML(XML(regionXML));
         }
      }
   }
}
