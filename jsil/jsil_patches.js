JSIL.ImplementExternals("Microsoft.Xna.Framework.Point", function ($) {
  $.Method({Static:false, Public:true }, "GetHashCode", 
    (new JSIL.MethodSignature($.Int32, [], [])), 
    function GetHashCode () {
      return this.x * 31 + this.y;;
    }
  );
});