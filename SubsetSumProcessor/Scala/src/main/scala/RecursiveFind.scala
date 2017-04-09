import scala.collection.parallel.mutable.ParArray


class FindSubset (arrCombo: Array[Double], arrLen: Int) {
  
  import util.control.Breaks._
  
  var isComplete: Boolean = false
  var result: String = ""
  
  def Find (currIndex: Int, currTotal: Double, currResult: String): Unit = {
  
     breakable {
      for (i <- currIndex to arrLen - 1){
      
         if (TestSubset(currTotal, arrCombo(i))){
           result = ExtendResult(currResult, i.toString())
           println("Result Found: " + result)
           isComplete = true
           break
         }
         else if (currTotal + arrCombo(i) > 0){
           
         }
         else if (currIndex < arrLen){
           var exResult = ExtendResult(currResult, i.toString())
           Find(i + 1, currTotal + arrCombo(i), exResult)
           if (isComplete) {
             break
           }
         }
         else{
           println("No result could be found")
         }   
      }
     }
  }
  
  private def TestSubset(currentTotal: Double, currentInt: Double): Boolean = {
    
    if (currentTotal + currentInt == 0) true else false
    
  }
  
  private def ExtendResult(currResult: String, newVal: String): String = {
    
    if (currResult == "") newVal else currResult + "," + newVal
    
  }
  
}

