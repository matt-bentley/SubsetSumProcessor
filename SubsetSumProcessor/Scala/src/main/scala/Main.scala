

object Main extends App{
  
  val inputs = new TransformArgs(args)
  
  val arrayCombo = inputs.ConvertToInt
  
  match{
    case Some(arrayCombo) => Run(arrayCombo)
    case None => println("Error - Please make sure all inputs are numeric")
  }
  
  def Run (array: Array[Int]) = {
    val subset = new DynamicFind(array)
    subset.FindSubset
  }
  
  
//  var i = 0
//  var arrayCombo: Array[Double] = new Array(args.length)
//  for (i <- 0 to args.length - 1){   
//      arrayCombo(i) = args(i).toDouble  
//  }
//  
//  val subset = new RecursiveFind(arrayCombo, arrayCombo.length)
//  
//  subset.Find(0, 0, "")
  
}