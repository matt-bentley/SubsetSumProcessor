import util.control.Breaks._
import scala.collection.parallel._

class DynamicFind (arrCombo: Array[Int]) {
  
  private val arrLen = arrCombo.length
  
  def FindSubset: Unit = {
    
    var a: Int = 0
    var b: Int = 0
    
    for (i <- 0 to arrLen - 1){
      if(arrCombo(i)>0) b += arrCombo(i) else a += arrCombo(i)
    }
    
    val s: Int = (b - a)+1
    
    val matrix = Array.ofDim[Boolean](arrLen,s) 
    
    matrix(0)(arrCombo(0) - a) = true  
    
    var findIndex: Int = 0
    
//    breakable {
//      val v = Vector.range(0, s - 1)
//      for (j <- 1 to arrLen - 1){
//        v.par.foreach { k =>     
//            val check = k  - arrCombo(j)
//            if(s - 1 >= check && check >= 0) {
//              if(matrix(j-1)(k) || (k + a) == arrCombo(j) || matrix(j-1)(check)) matrix(j)(k) = true
//            }
//            else{
//              if(matrix(j-1)(k) || (k + a) == arrCombo(j)) matrix(j)(k) = true
//            } 
//          }
//        if (matrix(j)(-a)) {
//          findIndex = j
//          break
//        }
//      }
//      findIndex = arrLen - 1
//    }
    
    var check: Int = 0
    
    breakable {
      for (j <- 1 to arrLen - 1){
        for (k <- 0 to s - 1){
          check = k  - arrCombo(j)
          if(s - 1 >= check && check >= 0) {
            if(matrix(j-1)(k) || (k + a) == arrCombo(j) || matrix(j-1)(check)) matrix(j)(k) = true
          }
          else{
            if(matrix(j-1)(k) || (k + a) == arrCombo(j)) matrix(j)(k) = true
          }
        }
        if (matrix(j)(-a)) {
          findIndex = j
          break
        }
      }
      findIndex = arrLen - 1
    }   
    
    val result: Boolean = matrix(findIndex)(-a)
    
     println(result)
    //println(matrix.deep.mkString("\n"))
    
    if (result){
      var findResult = List(arrCombo(findIndex).toDouble/100)
      var col: Int = -a - arrCombo(findIndex)
      for (i <- findIndex - 1 to 1 by -1){
        if (!matrix(i - 1)(col)){
          
          findResult :::= List(arrCombo(i).toDouble/100)
          col = col - arrCombo(i)
        }    
      }
      if (matrix(0)(col)) findResult :::= List(arrCombo(0).toDouble/100)
      println(findResult)
    }  
    
    
  }
  
}