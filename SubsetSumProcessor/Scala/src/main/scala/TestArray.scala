

class TestArraySum (arrayCombo: Array[Int]){
  
  def FindTotal: Int = {
    
    var i = 0
    var sum = 0
    
    for (i <- 0 to arrayCombo.length - 1){
    
      sum += arrayCombo(i)
    
    }   
    sum
  }
  
  def TestSubset(): Boolean = {
    
    if (arrayCombo(0) + arrayCombo(1) == 0) true else false
    
  }
  
}