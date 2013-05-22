  JSIL.ImplementExternals("Microsoft.Xna.Framework.Point", function ($) {

    var equalsImpl = function (lhs, rhs) {
      return lhs.X === rhs.X && lhs.Y === rhs.Y;
    };

    $.Method({Static:false, Public:true }, "Equals", 
      new JSIL.MethodSignature($.Boolean, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Point")], []), 
      function Equals (other) {
        return equalsImpl(this, other);
      }
    );

    $.Method({Static:false, Public:true }, "Object.Equals", 
      new JSIL.MethodSignature($.Boolean, [$.Object], []), 
      function Object_Equals (other) {
        return equalsImpl(this, other);
      }
    );

    $.Method({Static:false, Public:true }, "GetHashCode", 
      new JSIL.MethodSignature($.Int32, [], []), 
      function GetHashCode () {
        return this.x * 31 + this.y;
      }
    );

    $.Method({Static:false, Public:true }, "toString", 
      new JSIL.MethodSignature($.String, [], []), 
      function toString () {
        throw new Error('Not implemented');
      }
    );
  });

/*JSIL.ImplementExternals("Microsoft.Xna.Framework.Point", function ($) {
  $.Method({Static:false, Public:true }, "Equals", 
    (new JSIL.MethodSignature($.Boolean, [$xnaasms[0].TypeRef("Microsoft.Xna.Framework.Point")], [])), 
    function Equals (that) {
      return this.x === that.x && this.y === that.y;
    }
  );
});*/

JSIL.ImplementExternals("Microsoft.Xna.Framework.Rectangle", function ($) {
  $.Method({Static:false, Public:true }, "get_IsEmpty", 
    (new JSIL.MethodSignature($.Boolean, [], [])), 
    function get_IsEmpty () {
      return this.Width === 0 && this.Height === 0;
    }
  );
});