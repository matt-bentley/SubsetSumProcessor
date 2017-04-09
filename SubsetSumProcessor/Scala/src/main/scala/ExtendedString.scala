

class ExtendedString (s: String){
  def isNumber: Boolean = s.matches("[+-]?\\d+.?\\d+") || s.matches("[+-]?\\d+")
}

object ExtendedString {
  implicit def String2ExtendedString(s:String) = new ExtendedString(s)
}