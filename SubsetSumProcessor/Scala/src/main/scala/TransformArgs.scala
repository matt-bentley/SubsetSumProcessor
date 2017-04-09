

class TransformArgs (args: Array[String]) {
  
  import ExtendedString._
  
  private val arrLength = args.length
  
  def ConvertToInt: Option[Array[Int]] = {
    if (isNumeric){
      val arrayCombo: Array[Int] = new Array(arrLength)
      for (i <- 0 to args.length - 1){ 
        val double = Math.round(args(i).toDouble * 100)
        arrayCombo(i) = double.toInt 
      }
      return Some(arrayCombo)
    }
    else{
      return None
    }
  }

  
  private def isNumeric: Boolean = {
    if (args.forall(s => s.isNumber)) true else false
  }
  
  
}