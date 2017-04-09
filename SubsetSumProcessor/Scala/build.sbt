name := "Subset Sum Engine"

version := "0.1"

scalaVersion := "2.11.7"

fork := true

javaOptions += "-Xmx4G"

javaOptions += "-Xms2G"

libraryDependencies ++= Seq("org.scalatest" %% "scalatest" % "2.2.6" % "test",
"com.novocode" % "junit-interface" % "0.11" % "test")