import pyglet, sys

def playFile(file):
    sample = pyglet.media.load(file)
    sample.play()

print("Playing " + sys.argv[1])

if __name__ == "__main__":
    playFile(sys.argv[1])
