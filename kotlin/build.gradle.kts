plugins {
    kotlin("jvm") version "1.9.23"
}

group = "localids"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    testImplementation("io.kotest:kotest-runner-junit5:5.8.0")
    testImplementation("io.kotest:kotest-property:5.8.0")
}

tasks.test {
    useJUnitPlatform()
}
kotlin {
    jvmToolchain(19)
}