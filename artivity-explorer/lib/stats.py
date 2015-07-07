#! /usr/bin/python

from xml.dom import minidom

class EditingStatistics:
    """
    Helper class for calculating the editing statistics of a file.
    """
    def __init__(self):
        self.begin_edit_count = 0
        self.end_edit_count = 0
        self.edit_count = 0
        self.redo_count = 0
        self.undo_count = 0
        self.confidence = 0.0

    def calculate(self, treestore):
        event_types = dict()

        for row in treestore:
            t = row[2]

            if t in event_types:
                event_types[t] += 1
            else:
                event_types[t] = 1

        # Todo: Remove magic strings and replace by call to ontology member.
        if 'art:BeginEditingEvent' in event_types:
            self.begin_edit_count = event_types['art:BeginEditingEvent']

        if 'art:EndEditingEvent' in event_types:
            self.end_edit_count = event_types['art:EndEditingEvent']

        if 'art:EditEvent' in event_types:
            self.edit_count = event_types['art:EditEvent']

        if 'art:RedoEvent' in event_types:
            self.redo_count = event_types['art:RedoEvent']

        if 'art:UndoEvent' in event_types:
            self.undo_count = event_types['art:UndoEvent']

        if self.edit_count > 0:
            self.confidence = float(self.edit_count - self.undo_count + self.redo_count) / self.edit_count

class CompositionStatistics:
    def __init__(self):
        self.layer_count = 0
        self.group_count = 0
        self.element_count = 0
        self.masked_count = 0
        self.clipped_count = 0
        self.palette = set()

    def parse(self, filename):
        dom = minidom.parse(filename)

        for node in dom.childNodes:
            # Parse only the child nodes of the SVG root node
            self.parse_nodes(node.childNodes)

    def parse_nodes(self, nodelist):
        skip = ['metadata', 'sodipodi:namedview']

        for node in nodelist:
            # Only element and text nodes have a tag name
            if not node.nodeType == node.ELEMENT_NODE: continue

            # Skip tags that are no SVG elements.
            if node.tagName in skip: continue

            count = node.tagName != 'defs'

            self.parse_node(node, count)

    def parse_node(self, node, count):
        if node.tagName == 'g':
            mode = getattr(node, 'inkscape:groupmode')

            if mode == 'layer':
                self.layer_count += 1
            else:
                self.group_count += 1

        else:
            if hasattr(node, 'style'):
                style = getattr(node, 'style')

                if style.endswith(';'):
                    style = style[:-1]

                try:
                    attrs = dict(a.split(':') for a in style.split(';'))

                    if 'fill' in attrs:
                        self.add_colour(attrs['fill'])
                    if 'stroke' in attrs:
                        self.add_colour(attrs['stroke'])
                    if 'stop-color' in attrs:
                        self.add_colour(attrs['stop-color'])
                except ValueError:
                    print style

            clip = getattr(node, 'clip-path')
            mask = getattr(node, 'mask')

            if clip:
                self.clipped_count += 1
            elif mask and mask != 'none':
                self.masked_count += 1

            if count:
                self.element_count += 1

        for child in node.childNodes:
            # Only element and text nodes have a tag name
            if not child.nodeType == node.ELEMENT_NODE: continue

            self.parse_node(child, count)

    def add_colour(self, colour):
        if colour and colour.startswith('#'):
            self.palette.add(colour)

    def get_layer_count(self):
        return str(self.layer_count)

    def get_group_count(self):
        return str(self.group_count)

    def get_element_count(self):
        return str(self.element_count)

    def get_element_masked_count(self):
        return str(self.masked_count)

    def get_element_clipped_count(self):
        return str(self.clipped_count)

    def get_element_copied_count(self):
        return str('#')

    def get_element_duplicated_count(self):
        return str('#')

    def get_colour_count(self):
        return str(len(self.palette))

    def get_colour_palette(self):
        return self.palette

def getattr(node, name):
    if name in node.attributes.keys():
        return node.attributes[name].value
    else:
        return None

def hasattr(node, name):
    return name in node.attributes.keys()
