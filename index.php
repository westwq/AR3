<!doctype HTML>
<html>
<!--<script src="aframe-ar.js"> </script>-->

	<script src="https://aframe.io/releases/0.8.0/aframe.min.js"></script>
<script src="https://jeromeetienne.github.io/AR.js/aframe/build/aframe-ar.js"></script>
<script src="https://rawgit.com/donmccurdy/aframe-extras/master/dist/aframe-extras.loaders.min.js"></script><!-- for gltf 2.0 -->
	
<script>
	THREEx.ArToolkitContext.baseURL = 'https://rawgit.com/jeromeetienne/ar.js/master/three.js/'
</script>
<head>
	<style>
		#arjsDebugUIContainer{
			display : none;
		}
	</style>
</head>
	

<body style='margin : 0px; overflow: hidden;'>
  <a-scene embedded arjs='sourceType: webcam;'>
	  
	  <a-assets>
		<a-asset-item id="doll" src="docs/scene.gltf"></a-asset-item>
		  <a-asset-item id="cube" src="docs/Models/ninja.gltf"></a-asset-item>
		  
		  <a-asset-item id="knight" src="docs/Models/knight.gltf"></a-asset-item>
		  <a-asset-item id="mayan" src="docs/Models/mayan.gltf"></a-asset-item>
		  <a-asset-item id="centurion" src="docs/Models/centurion.gltf"></a-asset-item>
		  <a-asset-item id="ninja" src="docs/Models/ninja.gltf"></a-asset-item>
		  <a-asset-item id="viking" src="docs/Models/viking.gltf"></a-asset-item>
		  
		<a-asset-item id="Ani" src="docs/Text/ANI.gltf"></a-asset-item>
		<a-asset-item id="All" src="docs/Text/All5.gltf"></a-asset-item>
		<a-asset-item id="Csf" src="docs/Text/CSF.gltf"></a-asset-item>
		<a-asset-item id="Fi" src="docs/Text/FI.gltf"></a-asset-item>
		<a-asset-item id="Imgd" src="docs/Text/IMGD.gltf"></a-asset-item>
		  <a-asset-item id="Ict" src="docs/Text/ICT.gltf"></a-asset-item>
		  <a-asset-item id="It" src="docs/Text/IT.gltf"></a-asset-item>
		  <a-asset-item id="CICTP" src="docs/Text/CICTP.gltf"></a-asset-item>
		
	</a-assets>
	  
    <!-- handle marker with your own pattern -->
    <a-marker type='pattern' url='docs/pattern-hiro.patt'>
      <a-entity gltf-model="#cube" animation-mixer
		  position="0 0.2 0.5"
		  rotation="-90 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="Welcome to ICT" position="-0.5 0 0.8" rotation="-90 0 0" height="3" width="3" color="red"></a-text>	
    </a-marker>
	  
	  <a-marker type='pattern' url='docs/markerv6.patt'>
	    <a-entity gltf-model="#cube" animation-mixer
		  position="0 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="Test Custom Marker!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
    </a-marker>
	  
	  <a-marker type='pattern' url='docs/MTest.patt'>
	    <a-entity gltf-model="#cube" animation-mixer
		  position="0 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="MY OWN MARKER!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
    </a-marker>

    
	  
	  <!-- Start of handle RedCamp Markers here -->
    
	  <!-- old
	  <a-marker type='pattern' url='patterns/ICT_logo_pattern-marker.patt'>
	    <a-entity gltf-model="#knight" animation-mixer
		  position="0 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#All" animation-mixer
		  position="2 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="Welcome to ICT!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
    </a-marker>
	  
	  
	  <a-marker type='pattern' url='patterns/radio_pattern-marker.patt'>
	    <a-entity gltf-model="#knight" animation-mixer
		  position="0 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#Imgd" animation-mixer
		  position="2 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="Welcome to ICT!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
    </a-marker>
	  
	  
	  <a-marker type='pattern' url='patterns/redcamp_logo_pattern-marker.patt'>
	    <a-entity gltf-model="#mayan" animation-mixer
		  position="0 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#Fi" animation-mixer
		  position="2 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="Welcome to ICT!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
    </a-marker>
	  
	  -->
	  <!--
	  <a-marker type='pattern' url='patterns/singapore_pattern-marker.patt'>
	    <a-entity gltf-model="#centurion" animation-mixer
		  position="0 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#Csf" animation-mixer
		  position="2 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="Welcome to ICT!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
    </a-marker>
	  
	  <a-marker type='pattern' url='patterns/Alien_pattern-marker.patt'>
	    <a-entity gltf-model="#viking" animation-mixer
		  position="0 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#Ani" animation-mixer
		  position="2 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
	    
	<a-text value="Welcome to ICT!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
    </a-marker>
-->	  	  
	  
	  <!-- Infocomm Technology Marker -->
	  <a-marker type='pattern' url='docs/patterns2/dino_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#knight" animation-mixer
		  position="-4 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#Ict" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>
	
		  <!--<a-text value="Welcome to ICT!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
	-->   
		  
	</a-marker>
	  
	  <!-- ALL Marker -->
	  <a-marker type='pattern' url='docs/patterns2/redcamp_logo_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#centurion" animation-mixer
		  position="-4 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#All" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>
	    
	<!--<a-text value="Welcome to ICT!!" position="-0.5 0 0.5" rotation="-45 0 0" height="3" width="3" color="white"></a-text>	
	-->  
	  </a-marker>
	  
	  <!-- CSF Marker -->
	  <a-marker type='pattern' url='docs/patterns2/astronaut_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#mayan" animation-mixer
		  position="-1.5 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#Csf" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>
	    
	<a-text value="Cybersecurity and Digital Forensics" position="-1 0.5 0" rotation="0 0 0" height="5" width="5" color="red"></a-text>	
	 
	  </a-marker>
	  
	  <!-- IMGD Marker -->
	  <a-marker type='pattern' url='docs/patterns2/video_games_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#viking" animation-mixer
		  position="-2 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#Imgd" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>
	    
	<a-text value="Immersive Media and Game Design" position="-1 0.5 0" rotation="0 0 0" height="5" width="5" color="red"></a-text>	
	 
	  </a-marker>
	  
	  <!-- Ani Marker -->
	  <a-marker type='pattern' url='docs/patterns2/Alien_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#knight" animation-mixer
		  position="-1.5 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#Ani" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>
	    
	<a-text value="Animation" position="-1 0.5 0" rotation="0 0 0" height="5" width="5" color="red"></a-text>	
	
	  </a-marker>
	  
	  <!-- FI Marker -->
	  <a-marker type='pattern' url='docs/patterns2/dino_head_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#mayan" animation-mixer
		  position="-1 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#Fi" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>
	    
	<a-text value="Financial Informatics" position="-1 0.5 0" rotation="0 0 0" height="5" width="5" color="red"></a-text>	
	  
	</a-marker>
	  
	  <!-- IT Marker -->
	  <a-marker type='pattern' url='docs/patterns2/scop_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#ninja" animation-mixer
		  position="-1 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#It" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>
	    
	<a-text value="Information Technology" position="-1 0.5 0" rotation="0 0 0" height="5" width="5" color="red"></a-text>	
	    
	</a-marker>
	  
	  <!-- Common ICT Programme Marker -->
	  <a-marker type='pattern' url='docs/patterns2/singapore_pattern-marker.patt'>
	    <!--<a-entity gltf-model="#ninja" animation-mixer
		  position="-1 1 0"
		  scale="0.2 0.2 0.2"></a-entity>-->
		  
		  <a-entity gltf-model="#CICTP" animation-mixer
		  position="0 1 0"
		  scale="0.05 0.05 0.05"></a-entity>   
	</a-marker>
	  
	  
	  
	  <!-- Welcome to ICT Marker -->
	  <a-marker type='pattern' url='docs/patterns2/footballer_pattern-marker.patt'>
	    <a-entity gltf-model="#ninja" animation-mixer
		  position="-3 0.5 0"
		  rotation="-90 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#viking" animation-mixer
		  position="-1.5 0.5 0"
		  rotation="-90 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#mayan" animation-mixer
		  position="0 0.5 0"
		  rotation="-90 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#knight" animation-mixer
		  position="1.5 0.5 0"
		  rotation="-90 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
		  <a-entity gltf-model="#centurion" animation-mixer
		  position="3 0.5 0"
		  rotation="-90 0 0"
		  scale="0.2 0.2 0.2"></a-entity>
		  
	    
	<a-text value="Welcome to ICT!!" position="-1 0.5 1" rotation="-90 0 0" height="10" width="10" color="red"></a-text>	
	    
	</a-marker>
	  
	  
	  <!-- Dud Marker1 -->
	  <a-marker type='pattern' url='docs/patterns2/spider_pattern-marker.patt'>
	    <a-entity gltf-model="#ninja" animation-mixer
		  position="0 1 0"
		  scale="0.2 0.2 0.2"></a-entity>    
	</a-marker>
	  
	  <!-- Dud Marker2 -->
	  <a-marker type='pattern' url='docs/patterns2/ICT_logo_pattern-marker.patt'>
	    <a-entity gltf-model="#mayan" animation-mixer
		  position="0 1 0"
		  scale="0.2 0.2 0.2"></a-entity>    
	</a-marker>
	  
	  <!-- Dud Marker3 -->
	  <a-marker type='pattern' url='docs/patterns2/radio_pattern-marker.patt'>
	    <a-entity gltf-model="#viking" animation-mixer
		  position="0 1 0"
		  scale="0.2 0.2 0.2"></a-entity>    
	</a-marker>
	  
	  <!-- Dud Marker4 -->
	  <a-marker type='pattern' url='docs/patterns2/zebra_pattern-marker.patt'>
	    <a-entity gltf-model="#knight" animation-mixer
		  position="0 1 0"
		  scale="0.2 0.2 0.2"></a-entity>    
	</a-marker>
	  
	  <!-- Dud Marker5 -->
	  <a-marker type='pattern' url='docs/patterns2/unicon_pattern-marker.patt'>
	    <a-entity gltf-model="#centurion" animation-mixer
		  position="0 1 0"
		  scale="0.2 0.2 0.2"></a-entity>    
	</a-marker>
	  
	  
    <!-- add a simple camera -->
    <a-entity camera></a-entity>
	  
	  
  </a-scene>
</body>
</html>
