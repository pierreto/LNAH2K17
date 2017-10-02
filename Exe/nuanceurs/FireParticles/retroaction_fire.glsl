#version 410

uniform vec3 InitPos;
uniform float DeltaT, InitRadius, EndRadius, MaxSpeed, MaxTime, MaxHeight;

in vec3 Position;
in vec3 Speed;
in float TimeLeft;

out vec3 PositionMod;
out vec3 SpeedMod;
out float TimeLeftMod;


// Between  0 et UINT_MAX
uint randhash( uint seed ) {
    uint i=(seed^12345391u)*2654435769u;
    i ^= (i<<6u)^(i>>26u);
    i *= 2654435769u;
    i += (i<<5u)^(i>>12u);
    return i;
}

// Between  0 et 1
float myrandom( uint seed ) {
    const float UINT_MAX = 4294967295.0;
    return float(randhash(seed)) / UINT_MAX;
}

void main( void ) {
    // If particle out of range, reset it
    uint seed = uint(gl_VertexID);
    if (TimeLeft <= 0 || Position.y < (InitPos.y - MaxHeight)) {      
        float PI = 3.14159;
        float angle = myrandom(seed++) * 2.0 * PI;

        // Spawn particle at a random position in a circle
        PositionMod.x = InitPos.x + InitRadius * sin(angle) * myrandom(seed++);
        PositionMod.y = InitPos.y;
        PositionMod.z = InitPos.z + InitRadius * cos(angle) * myrandom(seed++);

        // Set random x,z end position
        vec3 randEndPos;
        randEndPos.x = InitPos.x + EndRadius * sin(angle) * myrandom(seed++);
        randEndPos.y = InitPos.y - MaxHeight;
        randEndPos.z = InitPos.z + EndRadius * cos(angle) * myrandom(seed++);

        // Set random speed
        float randSpeed = max(0.5, myrandom(seed++)) * MaxSpeed;
        SpeedMod = normalize(randEndPos - PositionMod) * randSpeed;

        // Set random lifespan
        TimeLeftMod = max(0.25, myrandom(seed++)) * MaxTime;
    }
    // Else move particle
    else {  
        PositionMod = Position + Speed * DeltaT;
        SpeedMod = Speed;
        TimeLeftMod = TimeLeft - DeltaT;
    }
}
